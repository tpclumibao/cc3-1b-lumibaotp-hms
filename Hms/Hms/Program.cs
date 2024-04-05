using System;
using System.Collections.Generic;

namespace Hms
{
    using System;
    using System.Collections.Generic;
    internal class Program
    {
        public enum RoomStyle
        {
            SingleRoom,
            DoubleRoom,
            TwinRoom,
            KingRoom,
            QueenRoom
        }

        public class HotelRoom
        {
            public int RoomNumber { get; }
            public RoomStyle Style { get; }
            public double Price { get; }
            public bool IsBooked { get; set; }

            public HotelRoom(int roomNumber, RoomStyle style, double price)
            {
                RoomNumber = roomNumber;
                Style = style;
                Price = price;
                IsBooked = false;
            }
        }

        public class Hotel
        {
            public string Name { get; }
            public string Location { get; }
            private List<HotelRoom> Rooms { get; }

            public Hotel(string name, string location, List<HotelRoom> rooms)
            {
                Name = name;
                Location = location;
                Rooms = rooms;
            }

            public void DisplayAvailableRooms()
            {
                Console.WriteLine($"Hotel {Name} - Available Rooms");
                foreach (var room in Rooms)
                {
                    if (!room.IsBooked)
                    {
                        Console.WriteLine($"  Room {room.RoomNumber}, Style: {room.Style}, Price: {room.Price}");
                    }
                }
            }

            public void DisplayBookedRooms()
            {
                Console.WriteLine($"Hotel {Name} - Booked Rooms");
                foreach (var room in Rooms)
                {
                    if (room.IsBooked)
                    {
                        Console.WriteLine($"  Room {room.RoomNumber}, Style: {room.Style}, Price: {room.Price}");
                    }
                }
            }
        }

        public abstract class User
        {
            public string Name { get; }
            public string Address { get; }
            public string Email { get; }
            public long PhoneNumber { get; }

            protected User(string name, string address, string email, long phoneNumber)
            {
                Name = name;
                Address = address;
                Email = email;
                PhoneNumber = phoneNumber;
            }
        }

        public class Guest : User
        {
            public List<Reservation> Reservations { get; }

            public Guest(string name, string address, string email, long phoneNumber) : base(name, address, email, phoneNumber)
            {
                Reservations = new List<Reservation>();
            }

            public void MakeReservation(Hotel hotel, HotelRoom room, DateTime startDate, DateTime endDate)
            {
                var reservation = new Reservation(startDate, endDate, room);
                Reservations.Add(reservation);
                room.IsBooked = true;
            }

            public void DisplayReservations()
            {
                Console.WriteLine($"List of Reservations of {Name}:");
                foreach (var reservation in Reservations)
                {
                    Console.WriteLine($"  {reservation}");
                }
            }
        }

        public class Receptionist : User
        {
            public Receptionist(string name, string address, string email, long phoneNumber) : base(name, address, email, phoneNumber)
            {
            }

            public void BookReservation(Guest guest, Reservation reservation)
            {
                guest.Reservations.Add(reservation);
                reservation.Room.IsBooked = true;
            }
        }

        public class Reservation
        {
            private static int reservationCounter = 1234567890;
            public int ReservationNumber { get; }
            public DateTime StartDate { get; }
            public DateTime EndDate { get; }
            public HotelRoom Room { get; }

            public Reservation(DateTime startDate, DateTime endDate, HotelRoom room)
            {
                ReservationNumber = reservationCounter++;
                StartDate = startDate;
                EndDate = endDate;
                Room = room;
            }

            public override string ToString()
            {
                return $"Reservation Number: {ReservationNumber} Start Time: {StartDate}, End Time: {EndDate}, Duration: {(EndDate - StartDate).Days}, Total: {Room.Price}";
            }
        }

        public class HotelManagementSystem
        {
            private List<Hotel> Hotels { get; }

            public HotelManagementSystem()
            {
                Hotels = new List<Hotel>();
            }

            public void AddHotel(Hotel hotel)
            {
                Hotels.Add(hotel);
            }

            public void DisplayHotels()
            {
                Console.WriteLine("List of Hotels:");
                foreach (var hotel in Hotels)
                {
                    Console.WriteLine($"  {hotel.Name}, {hotel.Location}");
                }
            }

            public void BookReservation(Hotel hotel, HotelRoom room, Guest guest, DateTime startDate, DateTime endDate)
            {
                guest.MakeReservation(hotel, room, startDate, endDate);
            }

            public void RegisterUser(User user)
            {
                Console.WriteLine($"User {user.Name} registered.");
            }

            public void DisplayReservationDetails(int reservationNumber)
            {
                Console.WriteLine($"Details of Reservation {reservationNumber}:");
            }

            public static void Main(string[] args)
            {
                List<HotelRoom> yananRooms = new List<HotelRoom>();
                HotelRoom room1 = new HotelRoom(101, RoomStyle.TwinRoom, 1500);
                HotelRoom room2 = new HotelRoom(102, RoomStyle.KingRoom, 3000);
                yananRooms.Add(room1);
                yananRooms.Add(room2);
                Hotel hotelYanan = new Hotel("Hotel Yanan", "123 GStreet, Takaw City", yananRooms);

                List<HotelRoom> hotel456Rooms = new List<HotelRoom>();
                HotelRoom hotel456Room1 = new HotelRoom(101, RoomStyle.QueenRoom, 2000);
                HotelRoom hotel456Room2 = new HotelRoom(102, RoomStyle.QueenRoom, 2000);
                hotel456Rooms.Add(hotel456Room1);
                hotel456Rooms.Add(hotel456Room2);
                Hotel hotel456 = new Hotel("Hotel 456", "Session Road, Baguio City", hotel456Rooms);

                HotelManagementSystem hms = new HotelManagementSystem();
                hms.AddHotel(hotelYanan);
                hms.AddHotel(hotel456);

                hms.DisplayHotels();

                hotelYanan.DisplayAvailableRooms();

                Guest terry = new Guest("Terry", "Addr 1", "terry@email.com", 63919129);
                hms.RegisterUser(terry);

                hms.BookReservation(hotelYanan, room1, terry, DateTime.Now, new DateTime(2024, 04, 16));

                hotelYanan.DisplayBookedRooms();

                terry.DisplayReservations();

                Receptionist anna = new Receptionist("Anna", "Addr 2", "anna@email.com", 67890);
                hms.RegisterUser(anna);

                Reservation res = new Reservation(new DateTime(2024, 05, 01), new DateTime(2024, 05, 06), hotel456Room2);
                anna.BookReservation(terry, res);

                terry.DisplayReservations();

                hms.DisplayReservationDetails(1234567890);
            }
        }

    }
}
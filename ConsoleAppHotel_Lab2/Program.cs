using ConsoleAppHotel_Lab2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections;


class Program
{
    static void Main(string[] args)
    {
        using HotelDbContext ctx = new();
        //Выполняем разные методы, содержащие операции выборки и изменения данных
        Console.WriteLine("==== Будут выполнены выборки данных (нажмите любую клавишу) ====");
        Console.ReadKey();
        Select(ctx);

        Console.WriteLine("==== Будут выполнены вставки записей (нажмите любую клавишу) ====\n");
        Console.ReadKey();
        Insert(ctx);

        Console.WriteLine("==== Будет выполнено удаление данных (нажмите любую клавишу) ====");
        Console.ReadKey();
        Delete(ctx);

        Console.WriteLine("====== Будет выполнено обновление данных (нажмите любую клавишу) ========");
        Console.ReadKey();
        Update(ctx);
    }

    static void Print(string sqltext, IEnumerable items)
    {
        Console.WriteLine(sqltext);
        Console.WriteLine("Записи: ");
        foreach (var item in items)
        {
            Console.WriteLine(item.ToString());
        }
        Console.WriteLine("\nНажмите любую клавишу\n");
        Console.ReadKey();
    }

    //Выборки данных | Задания 1-5
    static void Select(HotelDbContext db)
    {
        //1. Выборка всех записей из таблицы Rooms (Для наглядности будут выбраны 5 записей)
        var rooms = from room in db.Rooms
                    select new
                    {
                        //Для удобства будем выводить значения только некоторых полей
                        Код_номера = room.RoomId,
                        Тип_номера = room.RoomType,
                        Вместимость_номера = room.RoomCapacity,
                        Описание_номера = room.RoomDescription
                    };
        string comment = "1. Результат выполнения запроса на выборку записей из таблицы Rooms(номера) : \r\n";
        Print(comment, rooms.Take(5).ToList());


        //2. Выборка номеров с ценой меньше заданной (Для наглядности будут выбраны 5 запи-сей), используя методы расширений
        var roomsPrice = db.RoomPrices.Where(o => (o.RoomCost < 500))
           .OrderBy(o => o.RoomPriceId)
           .Select(gr => new
           {
               Код_стоимости_номреа = gr.RoomPriceId,
               Цена_Номера = gr.RoomCost,
               Код_номера = gr.RoomId
           }
           );

        comment = "2. Результат выполнения запроса на выборку записей с ценой < 500 из таблицы RoomPrices : \r\n";
        Print(comment, roomsPrice.Take(5).ToList());

        //3. Выборка данных из таблицы цены на номера, сгруппированных по коду номера номера, с выводом средней цены
        var roomsByPrice = from rp in db.RoomPrices
                           group rp by rp.RoomId into g
                           select new
                           {
                               Код_номера = g.Key,
                               Средняя_цена = g.Average(rp => rp.RoomCost)
                           };
        comment = "3. Выборка данных из таблицы цены на номера, сгруппированных по коду номера номера, с выводом средней цены:\r\n";
        Print(comment, roomsByPrice.Take(10).ToList());

        //4. Выборка данных из таблиц номера и цены на номера, с использованием методов расширения
        var rooms_roomPrices = from r in db.Rooms
                               join rp in db.RoomPrices on r.RoomId equals rp.RoomId
                               select new
                               {
                                   Код_номера = r.RoomId,
                                   Цена_номера = rp.RoomCost
                               };
        comment = "4. Результат выполнения запроса на выборку данных из таблиц Rooms и RoomPrices: \r\n";
        Print(comment, rooms_roomPrices.Take(5).ToList());


        //5. Выборка данных из номера и цены на номера, с фильтрацией по цене и типу
        var rooms_roomPrices_filter = from r in db.Rooms
                                      join rp in db.RoomPrices on r.RoomId equals rp.RoomId
                                      where r.RoomType == "Standard" && rp.RoomCost < 500
                                      select new
                                      {
                                          Код_номера = r.RoomId,
                                          Тип_номера = r.RoomType,
                                          Цена_номера = rp.RoomCost
                                      };
        comment = "5. Результат выполнения запроса на выборку данных из таблиц Rooms и RommPrices c ценой изделия < 500 и типом номера Standart: \r\n";
        Print(comment, rooms_roomPrices_filter.Take(5).ToList());

    }
    //Вставка данных
    static void Insert(HotelDbContext db)
    {
        // Создание нового номера
        Room room = new()
        {
            RoomId = 700,
            RoomType = "Standard",
            RoomCapacity = 4,
            RoomDescription = "This is room 993",
        };

        // Добавить в DbSet
        db.Rooms.Add(room);
        // Сохранить изменения в базе данных
        db.SaveChanges();

        Console.WriteLine($"6. В таблицу Materials успешно добавлена за-пись:\n\tRoomType:{room.RoomType}\n\tRoomCapacity:{room.RoomCapacity}\n\tRoomDescription:{room.RoomDescription}");

        // Создание новой цены номера
        RoomPrice roomPrice = new RoomPrice()
        {
            RoomPriceId = 700,
            RoomId = 700,
            RoomCost = 100.00m,
        };

        // Добавить в DbSet RoomPrices
        db.RoomPrices.Add(roomPrice);
        // Сохранить изменения в базе данных для таблицы RoomPrices
        db.SaveChanges();

        Console.WriteLine($"Запись успешно добавлена в таблицу RoomPrices:\n\tRoomPriceId: {roomPrice.RoomPriceId}\n\tRoomId: {roomPrice.RoomId}\n\tRoomCost: {roomPrice.RoomCost}");
    }

    //Удаление даных
    static void Delete(HotelDbContext db)
    {
        var roomId = 5;
        // Запись из таблицы Rooms
        var room = db.Rooms.FirstOrDefault(r => r.RoomId == roomId);

        // Запись из таблицы RoomPrices
        var roomPrice = db.RoomPrices.FirstOrDefault(rp => rp.RoomId == roomId);

        // Удаление записей из таблиц Rooms и RoomPrices
        if (room != null)
        {
            db.Rooms.Remove(room);
            db.SaveChanges();
            Console.WriteLine($"\nЗапись с id={roomId} успешно удалена из таблицы Rooms\n");
        }
        else
        {
            Console.WriteLine("\nВ таблице Rooms нет записи с указанным id!\n");
        }

        if (roomPrice != null)
        {
            db.RoomPrices.Remove(roomPrice);
            db.SaveChanges();
            Console.WriteLine($"\nЗапись с RoomId={roomId} успешно удалена из таблицы RoomPrices\n");
        }
        else
        {
            Console.WriteLine("\nВ таблице RoomPrices нет записи с указанным RoomId!\n");
        }
    }

    static void Update(HotelDbContext db)
    {
        var roomId = 520;
        // Подлежащая обновлению запись в таблице Rooms
        var room = db.Rooms.FirstOrDefault(r => r.RoomId == roomId);

        // Обновление
        if (room != null)
        {
            room.RoomCapacity = 3;
            // Сохранить изменения в базе данных
            db.SaveChanges();
            Console.WriteLine($"\nЗапись в таблице Rooms с RoomId={roomId} успешно обновлена: RoomCapacity изменен на {room.RoomCapacity}");
        }
        else
        {
            Console.WriteLine($"\nВ таблице Rooms нет записи с RoomId={roomId}!");
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elections
{
    public class Participants
    {
        public static Dictionary<string, string> Names = new[]
        {
            //2003 

            //new KeyValuePair<string, string>("1. \"ЕДИНЕНИЕ\"","ЕДИНЕНИЕ"), 
            new KeyValuePair<string, string>("2. \"СОЮЗ ПРАВЫХ СИЛ\"","СПС"),
            new KeyValuePair<string, string>("3. \"РОССИЙСКАЯ ПАРТИЯ ПЕНСИОНЕРОВ И ПАРТИЯ СОЦИАЛЬНОЙ СПРАВЕДЛИВОСТИ\"","РППиПСС"),
            new KeyValuePair<string, string>("4. \"Российская демократическая партия \"ЯБЛОКО\"","Яблоко"), 
            //new KeyValuePair<string, string>("5. \"За Русь Святую\"","За Русь Святую"), 
            //new KeyValuePair<string, string>("6. \"Объединенная Российская партия \"Русь\"","ОРП"), 
            //new KeyValuePair<string, string>("7. \"Новый курс - Автомобильная Россия\"","НКАР"), 
            //new KeyValuePair<string, string>("8. \"Народно-республиканская партия России\"","НРПР"), 
            //new KeyValuePair<string, string>("9. \"Российская экологическая партия \"Зеленые\"","РЭПЗ"), 
            new KeyValuePair<string, string>("10. \"Аграрная партия России\"","АПР"), 
            //new KeyValuePair<string, string>("11. \"Истинные патриоты России\"","ИПР"), 
            //new KeyValuePair<string, string>("12. \"НАРОДНАЯ ПАРТИЯ Российской Федерации\"","НПРФ"), 
            //new KeyValuePair<string, string>("13. \"Демократическая партия России\"","ДПР"), 
            //new KeyValuePair<string, string>("14. \"Великая Россия - Евразийский Союз\"","ВРЕС"), 
            //new KeyValuePair<string, string>("15. \"Партия СЛОН\"","СЛОН"), 
            //new KeyValuePair<string, string>("17. \"Партия Мира и Единства (ПМЕ)\"","ПМЕ"), 

            new KeyValuePair<string, string>("16. \"Родина\" (народно-патриотический союз)\"","Родина"),
            new KeyValuePair<string, string>("18. \"ЛДПР\"","ЛДПР"), 

            //new KeyValuePair<string, string>("19. \"Партия Возрождения России - Российская партия ЖИЗНИ\"","Жизнь"), 

            new KeyValuePair<string, string>("20. \"Политическая партия \"Единая Россия\"","Единая Россия"), 

            //new KeyValuePair<string, string>("21. \"Российская Конституционно-демократическая партия\"", "РКДП"),
            //new KeyValuePair<string, string>("22. \"Развитие предпринимательства\"", "РП"),

            new KeyValuePair<string, string>("23. \"Коммунистическая партия Российской Федерации (КПРФ)\"","КПРФ"), 

            ////////////////

            new KeyValuePair<string, string>("1. Политическая партия СПРАВЕДЛИВАЯ РОССИЯ","Справедливая Россия"),
            new KeyValuePair<string, string>("2. Политическая партия \"Либерально-демократическая партия России\"","ЛДПР"),
            new KeyValuePair<string, string>("3. Политическая партия \"ПАТРИОТЫ РОССИИ\"","Патриоты России")     ,
            new KeyValuePair<string, string>("4. Политическая партия \"Коммунистическая партия Российской Федерации\"","КПРФ"),
            new KeyValuePair<string, string>("5. Политическая партия \"Российская объединенная демократическая партия \"ЯБЛОКО\"","Яблоко")     ,
            new KeyValuePair<string, string>("6. Всероссийская политическая партия \"ЕДИНАЯ РОССИЯ\"","Единая Россия"),
            new KeyValuePair<string, string>("7. Всероссийская политическая партия \"ПРАВОЕ ДЕЛО\"","Правое Дело"),

            new KeyValuePair<string, string>("1.Политическая партия «Аграрная партия России»","Аграрная партия России"),
            new KeyValuePair<string, string>("2.Всероссийская политическая партия «Гражданская Сила»","Гражданская Сила"),
            new KeyValuePair<string, string>("3.Политическая партия «Демократическая партия России»","Демократическая партия России"),
            new KeyValuePair<string, string>("4.Политическая партия «Коммунистическая партия Российской Федерации»","КПРФ"),
            new KeyValuePair<string, string>("5.Политическая партия «СОЮЗ ПРАВЫХ СИЛ»","СОЮЗ ПРАВЫХ СИЛ"),
            new KeyValuePair<string, string>("6.Политическая партия «Партия социальной справедливости»","Партия социальной справедливости"),
            new KeyValuePair<string, string>("7.Политическая партия «Либерально-демократическая партия России»","ЛДПР"),
            new KeyValuePair<string, string>("8.Политическая партия «СПРАВЕДЛИВАЯ РОССИЯ: РОДИНА/ПЕНСИОНЕРЫ/ЖИЗНЬ»","Справедливая Россия"),
            new KeyValuePair<string, string>("9.Политическая партия «ПАТРИОТЫ РОССИИ»","Патриоты России"),
            new KeyValuePair<string, string>("10.Всероссийская политическая партия «ЕДИНАЯ РОССИЯ»","Единая Россия"),
            new KeyValuePair<string, string>("11.Политическая партия «Российская объединенная демократическая партия «ЯБЛОКО»","Яблоко"),

            /////////////////////
            //2016
            //new KeyValuePair<string, string>("\"1. ВСЕРОССИЙСКАЯ ПОЛИТИЧЕСКАЯ ПАРТИЯ \"\"РОДИНА\"\"\"", ""),
            //new KeyValuePair<string, string>("2. Политическая партия КОММУНИСТИЧЕСКАЯ ПАРТИЯ КОММУНИСТЫ РОССИИ\"", ""),
            //new KeyValuePair<string, string>("\"3. Политическая партия \"\"Российская партия пенсионеров за справедливость\"\"\"", ""),
            new KeyValuePair<string, string>("4. Всероссийская политическая партия \"ЕДИНАЯ РОССИЯ\"", "Единая Россия"),
            //new KeyValuePair<string, string>("\"5. Политическая партия \"\"Российская экологическая партия \"\"Зеленые\"\"\"", ""),
            //new KeyValuePair<string, string>("\"6. Политическая партия \"\"Гражданская Платформа\"\"\"", ""),
            new KeyValuePair<string, string>("7. Политическая партия ЛДПР - Либерально-демократическая партия России", "ЛДПР"),
            //new KeyValuePair<string, string>("\"8. Политическая партия \"\"Партия народной свободы\"\" (ПАРНАС)\"", ""),
            //new KeyValuePair<string, string>("\"9. Всероссийская политическая партия \"\"ПАРТИЯ РОСТА\"\"\"", ""),
            //new KeyValuePair<string, string>("\"10. Общественная организация Всероссийская политическая партия \"\"Гражданская Сила\"\"\"", ""),
            new KeyValuePair<string, string>("11. Политическая партия \"Российская объединенная демократическая партия \"ЯБЛОКО\"", "Яблоко"),	
            new KeyValuePair<string, string>("12. Политическая партия \"КОММУНИСТИЧЕСКАЯ ПАРТИЯ РОССИЙСКОЙ ФЕДЕРАЦИИ\"", "КПРФ"),
            //new KeyValuePair<string, string>("\"13. Политическая партия \"\"ПАТРИОТЫ РОССИИ\"\"\"", ""),
            new KeyValuePair<string, string>("14. Политическая партия СПРАВЕДЛИВАЯ РОССИЯ", "Справедливая Россия"),

            /////////////////////

            new KeyValuePair<string, string>("Глазьев Сергей Юрьевич", "Глазьев"),
            new KeyValuePair<string, string>("Малышкин Олег Александрович", "Малышкин"),
            new KeyValuePair<string, string>("Миронов Сергей Михайлович", "Миронов"),
            new KeyValuePair<string, string>("Путин Владимир Владимирович", "Путин"),
            new KeyValuePair<string, string>("Хакамада Ирина Муцуовна", "Хакамада"),
            new KeyValuePair<string, string>("Харитонов Николай Михайлович", "Харитонов"),
            new KeyValuePair<string, string>("Против всех", "Против всех"),

            new KeyValuePair<string, string>("Богданов Андрей Владимирович", "Богданов"),
            new KeyValuePair<string, string>("Жириновский Владимир Вольфович", "Жириновский"),
            new KeyValuePair<string, string>("Зюганов Геннадий Андреевич", "Зюганов"),
            new KeyValuePair<string, string>("Медведев Дмитрий Анатольевич", "Медведев"),
            new KeyValuePair<string, string>("Прохоров Михаил Дмитриевич", "Прохоров"),

            new KeyValuePair<string, string>("Грудинин Павел Николаевич", "Грудинин"),
            new KeyValuePair<string, string>("Собчак Ксения Анатольевна", "Собчак"),
            
            //////////////

        }.ToDictionary(k => k.Key, k => k.Value);
    }
}

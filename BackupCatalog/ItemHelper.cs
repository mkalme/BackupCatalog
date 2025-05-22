using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using FastStream;

namespace BackupCatalog {
    static class ItemHelper {
        //Deserialization
        public static Item GetItem(Type type, FastBinaryReader reader) {
            if (type == typeof(Group)) return Group.Deserialize(reader);
            if (type == typeof(DirectoryBackup)) return DirectoryBackup.Deserialize(reader);
            if (type == typeof(DirectoryBundle)) return DirectoryBundle.Deserialize(reader);
            if (type == typeof(FileBackup)) return FileBackup.Deserialize(reader);
            if (type == typeof(FileBundle)) return FileBundle.Deserialize(reader);

            return null;
        }

        //ID
        public static Type GetTypeFromID(this byte id) { 
            return ID.FirstOrDefault(x => x.Value == id).Key;
        }
        public static byte GetID(this Type type) {
            byte id;
            if (ID.TryGetValue(type, out id)) return id;

            return byte.MaxValue;
        }
        
        static readonly Dictionary<Type, byte> ID = new Dictionary<Type, byte>() {
            { typeof(Group), 0 },
            { typeof(DirectoryBackup), 1 },
            { typeof(DirectoryBundle), 2 },
            { typeof(FileBackup), 3 },
            { typeof(FileBundle), 4 },
        };
    }

    static class ItemExtensions {
        public static void AddChild<T>(this List<T> list, T item, ItemCollection parent) where T: Item {
            list.Add(item);
            item._parent = parent;
            item.PropertyChange += parent.PropertyChange;
        }
        public static bool RemoveChild<T>(this List<T> list, T item) where T : Item{
            for (int i = 0; i < list.Count; i++) {
                var remove = list[i];
                if (!Object.ReferenceEquals(remove, item)) continue;

                list.RemoveAt(i);
                remove._parent = null;

                return true;
            }

            return false;
        }

        public static bool Compare<T>(this List<T> list1, List<T> list2) where T : IEquatable<Item> {
            if (list1.Count != list2.Count) return false;

            for (int i = 0; i < list1.Count; i++) {
                if (!list1[i].Equals(list2[i])) return false;
            }

            return true;
        }
        public static List<T> CloneList<T>(this List<T> list) where T : ICloneable {
            List<T> output = new List<T>();

            foreach (var item in list) {
                output.Add((T)item.Clone());
            }

            return output;
        }

        public static string GetItemName<T>(this List<T> list, Type type, string fileName) where T : Item {
            List<Item> items = new List<Item>();

            foreach (var item in list) {
                if (item.GetType() == type) items.Add(item);
            }

            return items.GetNewName(fileName);
        }
        public static string GetNewName<T>(this List<T> list, string fileName) where T : Item {
            string output = fileName;

            int count = 0;
            while (list.FindIndex(x => x.Name == output) > -1) {
                count++;

                output = $"{fileName} ({count})";
            }

            return output;
        }

        public static DateTime Convert(this DateTime dateTime) {
            return dateTime == DateTime.MaxValue ? new DateTime(0) : dateTime;
        }
    }

    static class FastStreamExtensions {
        public static void Write(this FastBinaryWriter writer, DateTime dateTime) {
            writer.Write(dateTime.Ticks);
        }
        public static DateTime ReadDateTime(this FastBinaryReader reader) {
            return new DateTime(reader.ReadInt64());
        }
    }
}

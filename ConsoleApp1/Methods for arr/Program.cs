using System;

namespace Functions
{
    class Functions
    {
        static void Resize<T>(ref T[] arr, int newSize)
        {
            T[] newArr = new T[newSize];   
            for (int i = 0; i < arr.Length && i < newArr.Length; i++)
            {
                newArr[i] = arr[i];
            }

            arr = newArr;
        }

        static void Insert<T>(ref T[] arr, T value, int index)
        {
            T[] newArr = new T[arr.Length + 1];

            newArr[index] = value;

            for (int i = 0; i < index; i++)
            {
                newArr[i] = arr[i];
            }
            for (int i = index; i < arr.Length; i++)
            {
                newArr[i + 1] = arr[i];
            }
            arr = newArr;
        }

        static void AddFirst<T>(ref T[] arr, T value)
        {
            Insert(ref arr, value, 0);
        }

        static void AddLast<T>(ref T[] arr, T value)
        {
            Insert(ref arr, value, arr.Length);
        }

        static void Remove<T>(ref T[] arr, int index)
        {
            T[] newArray = new T[arr.Length - 1];

            for (int i = 0; i < index; i++)
            {
                newArray[i] = arr[i];
            }
            for (int i = index + 1; i < arr.Length; i++)
            {
                newArray[i - 1] = arr[i];
            }

            arr = newArray;
        }

        static void RemoveFirst<T>(ref T[] arr)
        {
            Remove(ref arr, 0);
        }

        static void RemoveLast<T>(ref T[] arr)
        {
            Remove(ref arr, arr.Length - 1);
        }

        static void Main(string[] args)
        {
            string[] arr = { "hello", "goodbye" };
            Resize(ref arr, 4);

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] ??= "0";
            }

            AddFirst(ref arr, "hey");

            AddLast(ref arr, "bye");

            Insert(ref arr, "nice", 4);

            Remove(ref arr, 6);

            RemoveFirst(ref arr);

            RemoveLast(ref arr);

            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine(arr[i]);
            }
        }
    }

}
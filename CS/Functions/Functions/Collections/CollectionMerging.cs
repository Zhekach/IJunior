using System;
using System.Collections.Generic;

namespace Functions.Collections
{
    internal class CollectionMerging
    {
        static void Main(string[] args)
        {
            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();
            List<string> listMerged = new List<string>();

            AddRandomElements(list1, 7, 15);
            AddRandomElements(list2, 5, 10);

            Console.WriteLine("First Collection");
            PrintList(list1);

            Console.WriteLine("Second Collection");
            PrintList(list2);

            MergeCollections(list1, listMerged);
            MergeCollections(list2, listMerged);

            Console.WriteLine("Merged Collection");
            PrintList(listMerged);
        }

        static void AddRandomElements(List<string> list, int elementsCount, int maxValue)
        {
            Random random = new Random();

            for (int i = 0; i < elementsCount; i++)
            {
                list.Add(random.Next(maxValue).ToString());
            }
        }

        static void PrintList(List<string> list)
        {
            foreach(string item in list)
            {
                Console.Write(item + ", ");
            }

            Console.WriteLine("\n");
        }

        static void MergeCollections(List<string> listBase, List<string> listMerged)
        {
            foreach (string item in listBase)
            {
                if(listMerged.Contains(item) == false)
                {
                    listMerged.Add(item);
                }
            }
        }
    }
}
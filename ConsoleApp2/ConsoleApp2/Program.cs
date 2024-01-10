namespace Lessons
{
    class Program    
    {        
        static void Main(string[] args)        
        {            
            var arr = new int[] { 3, 5, 3 };             
            uint even_num_count = 0;             
            for (var i = 0; i < arr.Length; i++)            
            {                
                if (arr[i] % 2 == 0)                
                {                    
                    even_num_count++;                
                }            
            }             
            Console.WriteLine(even_num_count);        
        }    
    }
}
using GameMechanics.Model;

namespace GameMechanics {
    public class Program {
        static void Main() 
            {
            //tester();
        }
        
        static void tester() {
            //int[][] field = new int[][] { 
            //                      [1,0,1,1,0],
            //                      [1,0,0,1,0],
            //                      [1,0,0,1,0],
            //                      [1,1,1,1,0],
            //                      [0,0,0,0,0]};
            //
            //Game game = new Game(field);
            //Console.WriteLine("--------------Before detection--------------");
            //printTable(game.field);
            //
            //game.Step(0, 1, 0, 2);
            //
            //Console.WriteLine("--------------After detection--------------");
            //printTable(game.field);
        }

        static void printTable(int[][] table) {
            foreach (int[] row in table) {
                Console.WriteLine(String.Join(",", row));
            }
            Console.WriteLine();
        }
    }
}

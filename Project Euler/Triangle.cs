using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Euler
{
    /*
     *Is a triangluar List of integers
     * */
    public class Triangle 
    {

        private int pivot { get { return data[0][0]; } }
        List<List<int>> data = new List<List<int>>();
        List<List<PathElement>> pathData;

        public Triangle(List<int> d)
        {
            int i = 0;
            int count = 0;
            foreach (int item in d)
            {
                if (count == i)
                {
                    count = 0;
                    i++;
                    data.Add(new List<int>());
                }
                data[i-1].Add(item);
                count++;
                
            }
        }

        private void convertDataToPathElement()
        {
            List<PathElement> newRow = new List<PathElement>();
            pathData = new List<List<PathElement>>();
            for (int i = 0; i < data.Count; i++)
            {
                newRow = new List<PathElement>();
                for (int j = 0; j <= i; j++)
                {
                    newRow.Add(new PathElement(data[i][j]));
                }
                pathData.Add(newRow);
            }
            
        }

        private List<bool> solveFromBottom()
        {
            //Starting for each bottom tile (starting second last row), 
            //find the maximum sum when adding with one of the tiles underneath
            //the sum achieved this way is then used by the tile above etc until
            //one reaches the peak
            convertDataToPathElement();
            for (int i = pathData.Count-2; i >= 0; i--)
            {
                for (int j = 0; j <= i; j++)
                {
                    pathData[i][j].add(pathData[i+1][j], pathData[i+1][j+1]);
                }
            }
            return pathData[0][0].Path;
        }

        public void printData()
        {
            foreach (List<int> row in data)
            {
                foreach (int item in row)
                {
                    Console.Write(item.ToString());
                }
                Console.WriteLine();
            }
        }


        private int giveWorth() //gives the sum of maximum values of each row
        {
            int temp = 0;
            foreach (List<int> row in data)
            {
                
                    temp += row.Max();
               
                
            }
            return temp;
        }

        private Triangle subTriangle(bool left = true)
        {
            if (data.Count == 1)
            {
                return new Triangle(new List<int>());
            }
            List<int> subdata = new List<int>();
            for (int i = 1; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].Count-1; j++)
                {
                    subdata.Add(data[i][j + (left ? 0 : 1)]);
                }
            }
            return new Triangle(subdata);
        }

        private List<bool> randomMouse()
        {
            //goes left or right with the propability of the value of the direction
            List<bool> path = new List<bool>();
            Triangle restTriangle = this;
            Random r = new Random();
            bool left;
            while (restTriangle.data.Count > 1)
            {
                left = r.NextDouble() <= (double)restTriangle.data[1][0] / (double)(restTriangle.data[1][0] + restTriangle.data[1][1]);
                restTriangle = restTriangle.subTriangle(left);
                path.Add(left);
            }
            return path;
        }

        private List<bool> getMostWorthyPath()
        {
            //the algorithm goes as follows:
            //the worth of a triangle gets determined by the sum of maximum elements of each row
            //when starting at the top of the triangle, one must decide to go either left or right
            //when going left, the whole right flank gets inaccessible and vice versa
            //the remaining elements are within the left or rigth "subtriangle". Choose the one
            //of higher worth. Repeat left/right decision. Etc.

            List<bool> path = new List<bool>();
            Triangle restTriangle = this;
            while (restTriangle.data.Count > 0)
            {
                //path.Add(restTriangle.data[0][0]);
                path.Add(restTriangle.subTriangle(true).giveWorth() > restTriangle.subTriangle(false).giveWorth() ? true : false);
                restTriangle = restTriangle.subTriangle(true).giveWorth() > restTriangle.subTriangle(false).giveWorth() ? restTriangle.subTriangle(true) : restTriangle.subTriangle(false);
                
            }
            return path;
        }

        private List<int> convertPath(List<bool> path)
        {
            Triangle rest = this;
            List<int> solution = new List<int>();
            foreach (bool way in path)
            {
                solution.Add(rest.pivot);
                rest = rest.subTriangle(way);
            }
            solution.Add(rest.pivot);
            return solution;
        }

        private int worthOfAPath(List<bool> path)
        {
            return convertPath(path).Sum();
        }

        public List<int> maximumPath()
        {



            /**
            //reverse dijkstra: start from each bottom element going greedy upwards
            //compare results and give back best

            List<List<int>> paths = new List<List<int>>();

            for (int i = 0; i < data[data.Count-1].Count; i++)
            {
                paths.Add(new List<int>());
                int x = i;
                for (int j = data.Count-1; j > 0; j--)
                {
                    if (x == 0)
                    {
                        paths[i].Add(data[j-1][x]);
                    }
                    else if (x==data[j-1].Count)
                    {
                        x--;
                        paths[i].Add(data[j-1][x]);
                    }
                    else
                    {
                        if (data[j-1][x]<data[j-1][x-1])
                        {
                            x--;
                        }
                        paths[i].Add(data[j - 1][x]);
                    }
                }

            }

            List<int> chosenOne = new List<int>();
            foreach (List<int> candidat in paths)
            {
                chosenOne = chosenOne.Sum() < candidat.Sum() ? candidat : chosenOne;
            }
            return chosenOne;
    */


            /**
             
            List<bool> solution = new List<bool>();

            List<List<bool>> candidates = new List<List<bool>>();
            candidates.Add(getMostWorthyPath());
            //after applying the subtriangle heuristic, each step which is not optimal
            //gets checked for a better, non-greedy alternative by comparing the achieved
            //value with the sum of worth already achieved and possible worth on second best way
            //which creates a new way to be also checked for second best alternatives etc
            //from all candidates the best one is selected

             for (int i = 0; i < candidates.Count; i++)
             {
                 int j = 0;
                 List<bool> way = candidates[i];
                 Triangle rest = this;
                 int accumulatedValue = pivot;
                 while (rest.giveWorth()!=rest.pivot)
                 {
                     j++;
                     if (accumulatedValue+rest.subTriangle(!way[j]).giveWorth()>worthOfAPath(way))
                     {
                         List<bool> newWay = new List<bool>();
                         for (int k = 0; k < j; k++)
                         {
                             newWay.Add(way[k]);
                         }
                         //newWay.Add(!way[j]);
                         foreach (bool pathway in rest.subTriangle(!way[j]).getMostWorthyPath())
                         {
                             newWay.Add(pathway);
                         }
                         candidates.Add(newWay);
                     }

                     rest = rest.subTriangle(way[j]);
                     accumulatedValue += rest.pivot;
                 }
             }


             foreach (List<bool> candidate in candidates)
             {
                 solution = convertPath(candidate).Sum() > convertPath(solution).Sum() ? candidate : solution; 
             }

             return convertPath(solution);
             */

            /**
            int max;
            List<bool> bestGuess = new List<bool>();
            bestGuess = randomMouse();
            List<bool> newTry = new List<bool>();
            max = convertPath(bestGuess).Sum();
            for (int i = 0; i < 10000000; i++)
            {
                newTry = randomMouse();
                if (convertPath(newTry).Sum() > convertPath(bestGuess).Sum())
                {
                    bestGuess = newTry;
                    convertPath(bestGuess).Sum();
                }
            }*/

            return convertPath(solveFromBottom());
        }
    }
}

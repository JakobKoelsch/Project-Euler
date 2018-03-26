using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Euler
{
    class PathElement
    {
        private List<bool> path;
        private int v;

        public int Worth { get { return v; } }
        public List<bool> Path { get { return path; } }

        public PathElement(int v)
        {
            this.path = new List<bool>();
            this.v = v;
        }

        public void add(PathElement leftern, PathElement rightern)
        {
            if (leftern.Worth >= rightern.Worth)
            {
                v += leftern.Worth;
                foreach (bool way in leftern.path)
                {
                    if (way)
                    {
                        path.Add(true);
                        
                    }
                    else
                    {
                        path.Add(false);
                    }
                }
                path.Insert(0, true);
            }
            else
            {
                v += rightern.Worth;
                foreach(bool way in rightern.path)
                {
                    if (way)
                    {
                        path.Add(true);

                    }
                    else
                    {
                        path.Add(false);
                    }
                }
                path.Insert(0, false);
            }
            
        }
    }
}

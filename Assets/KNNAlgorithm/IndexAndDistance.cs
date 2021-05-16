using System;
using System.Collections.Generic;
using System.Text;

namespace KnnConsoleAppForUnity
{
    //används av knn
    public class IndexAndDistance: IComparable<IndexAndDistance>
    {
        public int idx; // lable or class of a point
        public double dist; //distance to Uknown

        //sort to find k closest with help from Icomparable
        public int CompareTo(IndexAndDistance other)
        {
            if (this.dist < other.dist) return -1;
            else if (this.dist > other.dist) return +1;
            else return 0;
        }
    }
}

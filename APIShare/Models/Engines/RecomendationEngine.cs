using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace APIShare.Models.Engines
{
    public class RecomendationEngine
    {
        private int[] AllLibraries { get; set; }
        private UserRecommendation[] Users { get; set; }
        private UserRecommendation CurrentUser { get; set; }
        private int[] CurrentUserSkills { get; set; }
        private int MatrixSize { get; set; }

        /// <summary>
        /// This constructor sets each data point needed to run the engine
        /// </summary>
        /// <param name="inputParams">Parameters for the engine defined in RecommendationInput class</param>
        public RecomendationEngine(RecommendationInput inputParams)
        {
            this.AllLibraries = inputParams.LibraryIDs;
            this.Users = inputParams.Users;
            this.CurrentUser = inputParams.CurrentUser;
            this.CurrentUserSkills = inputParams.CurrentUserSkills;
            this.MatrixSize = this.AllLibraries.Length;
        }

        /// <summary>
        /// Runs the engine and gives back sorted list of library ids
        /// </summary>
        /// <returns>int[] : sorted array of LibraryID's in likelyness of the user wanting it</returns>
        public int[] RunRecommendationEngine()
        {
            //POPULATE A DOUBLE ARRAY THAT WILL BE THE TRANSITION MATRIX
            double[,] tMatrix = new double[this.MatrixSize, this.MatrixSize];
            for(int i = 0; i < this.MatrixSize; i++)
            {
                for(int j = 0; j < this.MatrixSize; j++)
                {
                    int connectionCount = 0;
                    if (i != j)
                    {
                        foreach (var user in this.Users)
                        {
                            if (user.LikedLibraries[this.AllLibraries[i]] == true && user.LikedLibraries[this.AllLibraries[j]] == true)
                            {
                                connectionCount++;
                            }
                        }
                    }
                    tMatrix[i, j] = connectionCount;
                }
            }

            double[] currentUser = new double[this.MatrixSize];
            for (int i = 0; i < this.MatrixSize; i++)
            {
                if (CurrentUser.LikedLibraries[AllLibraries[i]] == true)
                {
                    currentUser[i] = 1;
                }
                else
                {
                    currentUser[i] = 0;
                }
            }
            Matrix<double> transitionMatrix = DenseMatrix.OfArray(tMatrix);
            Vector<double> u = DenseVector.OfArray(currentUser);
            u.Normalize(1.0);
            transitionMatrix.NormalizeRows(1.0);
            var v = u.ToColumnMatrix();
            double c = .2;

            double epsilon = 1;

            while (epsilon > .005)
            {
                var v1 = v;
                v = transitionMatrix.Multiply(v).Multiply(1 - c).Add(u.ToColumnMatrix().Multiply(c));
                var differenceVector = v1.Subtract(v);
                //SET EPSILON TO THE SUM OF THE ABSOLUTE VALUEa
                epsilon = 0;
                var differenceEnumerable = differenceVector.Enumerate();
                foreach(var entry in differenceEnumerable)
                {
                    epsilon += Math.Abs(entry);
                }
            }

            var likelynessVector = v.Enumerate();
            //LOOK AT THE ORDER
            var length = likelynessVector.Count();
            int[] orderedIds = new int[length];

            //Sort through the array then find highest
            //Add the corrusponding id into the new orderedIds array
            //Set the old one in the likelynessVector to 0
            var lv = likelynessVector.ToArray();
            for (int i = 0; i < lv.Length; i++ )
            {
                int highestIndex = 0;
                double highestVal = -1;
                for(int j = 0; j < lv.Length; j++)
                {
                    //If this current likelieness is higher, set it
                    if(lv[j] > highestVal)
                    {
                        highestVal = lv[j];
                        highestIndex = j;
                    }
                }
                orderedIds[i] = this.AllLibraries[highestIndex];
                lv[highestIndex] = -1;
            }

            return orderedIds;
        }

    }

    public class RecommendationInput
    {
        public int[] LibraryIDs { get; set; }
        public UserRecommendation[] Users { get; set; }
        public UserRecommendation CurrentUser { get; set; }
        public int[] CurrentUserSkills { get; set; }
    }

    public class UserRecommendation
    {
        public int UserID { get; set; }
        //Dictionary with key of LibraryID and bool if it is liked by this user
        public Dictionary<int, bool> LikedLibraries { get; set; }
    }

}
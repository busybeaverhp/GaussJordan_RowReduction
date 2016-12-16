using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GaussJordanRREFLib
{
    public class Matrix
    {
        public BigInteger[,][] jma;

        public BigInteger[,][] InitializeJMA(int arrayLength, int rowDim, int columnDim)
        {
            BigInteger[,][] localJMA = new BigInteger[rowDim, columnDim][];

            for (int i = 0; i < localJMA.GetLength(0); i++)
            {
                for (int j = 0; j < localJMA.GetLength(1); j++)
                {
                    localJMA[i, j] = new BigInteger[3];
                }
            }

            localJMA[0, 0][1] = 23;
            localJMA[0, 1][1] = 07;
            localJMA[0, 2][1] = 03;
            localJMA[0, 3][1] = 1147;
            localJMA[1, 0][1] = 05;
            localJMA[1, 1][1] = 19;
            localJMA[1, 2][1] = 17;
            localJMA[1, 3][1] = 1263;
            localJMA[2, 0][1] = 13;
            localJMA[2, 1][1] = 01;
            localJMA[2, 2][1] = 11;
            localJMA[2, 3][1] = 0851;

            for (int i = 0; i < localJMA.GetLength(0); i++)
            {
                for (int j = 0; j < localJMA.GetLength(1); j++)
                {
                    localJMA[i, j][2] = 1;
                }
            }
            return localJMA;
        }

        public void DisplayJMA()
        {
            string localResult;

            for (int i = 0; i < jma.GetLength(0); i++)
            {
                for (int j = 0; j < jma.GetLength(1); j++)
                {
                    if (jma[i, j][2] == 1)
                    {
                        localResult = (jma[i, j][1].ToString().PadLeft(8));
                        Console.Write("{0}", localResult);
                    }

                    else
                    {
                        localResult = (jma[i, j][1].ToString() + "/" + jma[i, j][2].ToString()).PadLeft(8);
                        Console.Write("{0}", localResult);
                    }
                }
                Console.WriteLine();
            }
        }
    }

    public class MatrixOperator
    {
        public static BigInteger[,][] AttemptRref(BigInteger[,][] matrix)
        {
            BigInteger[,][] localMatrix = matrix;
            bool[] usedRows = new bool[matrix.GetLength(0)];

            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    if (usedRows[i] == false && matrix[i, j][1] != 0)
                    {
                        localMatrix = ColumnEliminationAddition(localMatrix, i, j);
                        usedRows[i] = true;
                    }
                }
            }

            localMatrix = SetIdentityOne(localMatrix);

            return localMatrix;
        }

        private static BigInteger[,][] SetIdentityOne(BigInteger[,][] matrix)
        {
            BigInteger[,][] localMatrix = matrix;
            BigInteger[] localDivisor = new BigInteger[3];
            int identityTest = 0;
            bool[] usedRows = new bool[localMatrix.GetLength(0)];

            for (int i = 0; i < localMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < (localMatrix.GetLength(1) - 1); j++)
                {
                    if (localMatrix[i, j][1] != 0)
                    {
                        identityTest += 1;
                        localDivisor = localMatrix[i, j];
                    }
                }

                if (identityTest == 1)
                {
                    for (int j = 0; j < localMatrix.GetLength(1); j++)
                    {
                        if (localMatrix[i, j][1] != 0)
                        {
                            localMatrix[i, j] = Fraction.Divide(localMatrix[i, j], localDivisor);
                        }
                    }
                }

                identityTest = 0;
            }

            return localMatrix;
        }

        private static BigInteger[,][] ColumnEliminationAddition(BigInteger[,][] matrix, int fromRowIndex1,
            int toTargetColumn)
        {
            BigInteger[,][] localMatrix = matrix;

            for (int i = 0; i < localMatrix.GetLength(0); i++)
            {
                localMatrix = EliminationAddition(localMatrix, fromRowIndex1, i, toTargetColumn);
            }

            return localMatrix;
        }

        private static BigInteger[,][] EliminationAddition(BigInteger[,][] matrix, int fromRowIndex1, int toTargetRow,
            int toTargetColumn)
        {
            BigInteger[,][] localMatrix = matrix;

            #region Activation when target row is different from instigator row

            if (localMatrix[fromRowIndex1, toTargetColumn][1] != 0 && fromRowIndex1 != toTargetRow)
            {
                BigInteger[] localMultiplier = Fraction.Divide(localMatrix[toTargetRow, toTargetColumn],
                    localMatrix[fromRowIndex1, toTargetColumn]);
                localMultiplier[1] = localMultiplier[1]*-1;

                BigInteger[,][] transformedRow = new BigInteger[1, localMatrix.GetLength(1)][];

                for (int i = 0; i < localMatrix.GetLength(1); i++)
                {
                    transformedRow[0, i] = Fraction.Multiply(localMatrix[fromRowIndex1, i], localMultiplier);
                }

                for (int i = 0; i < localMatrix.GetLength(1); i++)
                {
                    localMatrix[toTargetRow, i] = Fraction.Add(transformedRow[0, i], localMatrix[toTargetRow, i]);
                }
            }

            #endregion

            return localMatrix;
        }

        private static BigInteger[,][] SwapRows(BigInteger[,][] matrix, int rowIndex1, int rowIndex2)
        {
            BigInteger[,][] localMatrix = matrix;
            BigInteger[,][] localTempHolderMatrix = new BigInteger[2, 4][];

            for (int i = 0; i < localMatrix.GetLength(1); i++)
            {
                localTempHolderMatrix[0, i] = localMatrix[rowIndex2, i];
                localTempHolderMatrix[1, i] = localMatrix[rowIndex1, i];
            }

            for (int i = 0; i < localMatrix.GetLength(1); i++)
            {
                localMatrix[rowIndex1, i] = localTempHolderMatrix[0, i];
                localMatrix[rowIndex2, i] = localTempHolderMatrix[1, i];
            }

            return localMatrix;
        }

        private static BigInteger[,][] AddRow(BigInteger[,][] matrix, int fromRowIndex1, int toRowIndex2)
        {
            BigInteger[,][] localMatrix = matrix;

            for (int i = 0; i < localMatrix.GetLength(1); i++)
            {
                localMatrix[toRowIndex2, i] = Fraction.Add(localMatrix[fromRowIndex1, i], localMatrix[toRowIndex2, i]);
            }

            return localMatrix;
        }
    }

    public class Fraction
    {
        #region Addition

        public static BigInteger[] Add(BigInteger[] fraction1, BigInteger[] fraction2)
        {
            BigInteger[] localResult;
            localResult = Add(fraction1[1], fraction1[2], fraction2[1], fraction2[2]);
            return localResult;
        }

        public static BigInteger[] Add(BigInteger numerator1, BigInteger numerator2)
        {
            BigInteger[] localResult;
            localResult = Add(numerator1, 1, numerator2, 1);
            return localResult;
        }

        public static BigInteger[] Add(BigInteger numerator1, BigInteger denominator1,
            BigInteger numerator2, BigInteger denominator2)
        {
            BigInteger[] localResult = new BigInteger[3];

            localResult[0] = 1;
            localResult[1] = (numerator1*denominator2) + (numerator2*denominator1);
            localResult[2] = (denominator1*denominator2);

            localResult = Reduction(localResult);

            return localResult;
        }

        #endregion

        #region Subtraction

        public static BigInteger[] Subtract(BigInteger[] fraction1, BigInteger[] fraction2)
        {
            BigInteger[] localResult;
            localResult = Subtract(fraction1[1], fraction1[2], fraction2[1], fraction2[2]);
            return localResult;
        }

        public static BigInteger[] Subtract(BigInteger numerator1, BigInteger numerator2)
        {
            BigInteger[] localResult;
            localResult = Subtract(numerator1, 1, numerator2, 1);
            return localResult;
        }

        public static BigInteger[] Subtract(BigInteger numerator1, BigInteger denominator1,
            BigInteger numerator2, BigInteger denominator2)
        {
            BigInteger[] localResult = new BigInteger[3];

            localResult[0] = 2;
            localResult[1] = (numerator1*denominator2) - (numerator2*denominator1);
            localResult[2] = (denominator1*denominator2);

            localResult = Reduction(localResult);

            return localResult;
        }

        #endregion

        #region Multiplication

        public static BigInteger[] Multiply(BigInteger[] fraction1, BigInteger[] fraction2)
        {
            BigInteger[] localResult;
            localResult = Multiply(fraction1[1], fraction1[2], fraction2[1], fraction2[2]);
            return localResult;
        }

        public static BigInteger[] Multiply(BigInteger numerator1, BigInteger numerator2)
        {
            BigInteger[] localResult;
            localResult = Multiply(numerator1, 1, numerator2, 1);
            return localResult;
        }

        public static BigInteger[] Multiply(BigInteger numerator1, BigInteger denominator1,
            BigInteger numerator2, BigInteger denominator2)
        {
            BigInteger[] localResult = new BigInteger[3];

            localResult[0] = 3;
            localResult[1] = (numerator1*numerator2);
            localResult[2] = (denominator1*denominator2);

            localResult = Reduction(localResult);

            return localResult;
        }

        #endregion

        #region Division

        public static BigInteger[] Divide(BigInteger[] fraction1, BigInteger[] fraction2)
        {
            BigInteger[] localResult;
            localResult = Divide(fraction1[1], fraction1[2], fraction2[1], fraction2[2]);
            return localResult;
        }

        public static BigInteger[] Divide(BigInteger numerator1, BigInteger numerator2)
        {
            BigInteger[] localResult;
            localResult = Divide(numerator1, 1, numerator2, 1);
            return localResult;
        }

        public static BigInteger[] Divide(BigInteger numerator1, BigInteger denominator1,
            BigInteger numerator2, BigInteger denominator2)
        {
            BigInteger[] localResult = new BigInteger[3];

            localResult[0] = 4;
            localResult[1] = (numerator1*denominator2);
            localResult[2] = (denominator1*numerator2);

            localResult = Reduction(localResult);

            return localResult;
        }

        #endregion

        private static BigInteger[] Reduction(BigInteger[] fractionValue)
        {
            BigInteger[] result = fractionValue;
            BigInteger gcd = BigInteger.GreatestCommonDivisor(result[1], result[2]);

            result[1] = result[1]/gcd;
            result[2] = result[2]/gcd;

            return result;
        }
    }
}
using System;

namespace TaskManager.Common.Helpers
{
    public static class ArrayExtension
    {
        public static T[][] AsDoubleArray<T>(this T[] array, int columnsCount)
        {
            var rowsCount = (int) Math.Ceiling(((double) array.Length / columnsCount));

            var result = new T[rowsCount][];

            for (var column = 0; column < rowsCount; column++)
            {
                var rowsInColumn = Math.Min(array.Length - (column * columnsCount), columnsCount);
                result[column] = new T[columnsCount];
                for (var row = 0; row < rowsInColumn; row++)
                {
                    result[column][row] = array[column * columnsCount + row];
                }
            }

            return result;
        }
    }
}
namespace sharpkmeans;

public static class Utils
{
    public static float[][] CreateJaggedArray2D(int len1, int len2)
    {
        float[][] array = new float[len1][];

        for (int i = 0; i < len1; i++)
        {
            array[i] = new float[len2];
        }

        return array;
    }
}
using Food_Industry;
using System.Collections;
using System.Collections.Generic;

public static class FISLoader
{
    static FIS instance = null;

    public static void SetInstance(FIS fis)
    {
        instance = fis;
    }

    public static FIS GetFIS()
    {
        if(instance == null)
        {
            instance = new FIS(0, 100, ColorGenerator.GetAllColor()[0], MEnv.default_cpu_num);
        }

        return instance;
    }
}

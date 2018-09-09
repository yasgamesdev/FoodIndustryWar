using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class TechnologyData
    {
        public TechnologyType type { get; private set; }
        public TechnologyClassification classification { get; private set; }
        public double probability { get; private set; }
        public double frequency { get; private set; }
        public double min_magnification { get; private set; }
        public double sigma { get; private set; }

        public TechnologyData(TechnologyType type, TechnologyClassification classification, double probability, double frequency, double min_magnification, double sigma)
        {
            this.type = type;
            this.classification = classification;
            this.probability = probability;
            this.frequency = frequency;
            this.min_magnification = min_magnification;
            this.sigma = sigma;
        }

        public static Dictionary<Enum, object> GetTechnologyData()
        {
            Dictionary<Enum, object> data = new Dictionary<Enum, object>();

            data.Add(TechnologyType.Agrichemical, new TechnologyData(TechnologyType.Agrichemical, TechnologyClassification.Agricultural, 1.0, 1.0, 0.2, 0.8));
            data.Add(TechnologyType.Fertilizer, new TechnologyData(TechnologyType.Agrichemical, TechnologyClassification.Agricultural, 1.0, 1.0, 0.2, 0.8));
            data.Add(TechnologyType.Aquaculture, new TechnologyData(TechnologyType.Aquaculture, TechnologyClassification.Fishing, 1.0, 1.0, 0.2, 0.8));
            data.Add(TechnologyType.CrabBasket, new TechnologyData(TechnologyType.CrabBasket, TechnologyClassification.Fishing, 1.0, 1.0, 0.2, 0.8));
            data.Add(TechnologyType.FishingBoat, new TechnologyData(TechnologyType.FishingBoat, TechnologyClassification.Fishing, 1.0, 1.0, 0.2, 0.8));
            data.Add(TechnologyType.MeatPacking, new TechnologyData(TechnologyType.MeatPacking, TechnologyClassification.Processing, 1.0, 1.0, 0.2, 0.8));
            data.Add(TechnologyType.Domesticated, new TechnologyData(TechnologyType.Domesticated, TechnologyClassification.AnimalBreeding, 1.0, 1.0, 0.2, 0.8));
            data.Add(TechnologyType.Salt, new TechnologyData(TechnologyType.Salt, TechnologyClassification.Processing, 1.0, 1.0, 0.2, 0.8));
            data.Add(TechnologyType.Sugar, new TechnologyData(TechnologyType.Sugar, TechnologyClassification.Processing, 1.0, 1.0, 0.2, 0.8));
            data.Add(TechnologyType.BreadMachine, new TechnologyData(TechnologyType.BreadMachine, TechnologyClassification.Machine, 1.0, 1.0, 0.2, 0.8));
            data.Add(TechnologyType.NoodleMachine, new TechnologyData(TechnologyType.NoodleMachine, TechnologyClassification.Machine, 1.0, 1.0, 0.2, 0.8));
            data.Add(TechnologyType.OilWringers, new TechnologyData(TechnologyType.OilWringers, TechnologyClassification.Machine, 1.0, 1.0, 0.2, 0.8));
            data.Add(TechnologyType.FoodProcessingMachinery, new TechnologyData(TechnologyType.FoodProcessingMachinery, TechnologyClassification.Machine, 1.0, 1.0, 0.2, 0.8));
            data.Add(TechnologyType.Feed, new TechnologyData(TechnologyType.Feed, TechnologyClassification.Agricultural, 1.0, 1.0, 0.2, 0.8));
            data.Add(TechnologyType.Cheese, new TechnologyData(TechnologyType.Cheese, TechnologyClassification.Processing, 1.0, 1.0, 0.2, 0.8));
            data.Add(TechnologyType.Management, new TechnologyData(TechnologyType.Management, TechnologyClassification.EatingOut, 1.0, 1.0, 0.2, 0.8));

            return data;
        }
    }
}

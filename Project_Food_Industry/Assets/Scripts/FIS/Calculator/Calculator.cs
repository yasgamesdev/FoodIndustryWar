using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Food_Industry
{
    public class Calculator : ID
    {
        CalculatorState state;
        bool stop;

        Prices prices;
        Dictionary<int, Score> scores;

        public Calculator(FIS fis) : base(fis)
        {
            state = CalculatorState.Stop;
        }

        [JsonConstructor]
        public Calculator(Reference<FIS> ref_fis, int id) : base(ref_fis, id)
        {
            state = CalculatorState.Stop;
        }

        void SetScores()
        {
            prices = GetMarketPrice().GetDeepCopy();

            scores = new Dictionary<int, Score>();
            foreach (int product_id in GetResourcesCaches().GetMarketProductID())
            {
                scores.Add(product_id, new Score(prices, product_id, GetFIS()));
            }
        }

        public void Calculate()
        {
            state = CalculatorState.Calculating;
            stop = false;

            SetScores();

            Task.Run(() => {
                var tasks = new List<Task>();
                foreach (Score score in scores.Values)
                {
                    tasks.Add(Task.Factory.StartNew(score.Calculate));
                }
                Task.WaitAll(tasks.ToArray());

                int count = 0;
                while (!stop)
                {
                    if (scores.Values.All(x => x.score <= 1.0))
                    {
                        break;
                    }
                    else
                    {
                        double max_score = scores.Values.Max(x => x.score);
                        Score score = scores.Values.First(x => x.score == max_score);

                        //Console.Write($"{count++}:");
                        score.SetOptimizedPrice();

                        tasks.Clear();
                        foreach (int product_id in score.GetAffected())
                        {
                            tasks.Add(Task.Factory.StartNew(scores[product_id].Calculate));
                        }
                        Task.WaitAll(tasks.ToArray());
                    }
                }

                state = CalculatorState.Stop;

                Lib.Debug("Stop");
            });
        }

        public CalculatorState GetState()
        {
            return state;
        }

        public void Stop()
        {
            stop = true;
        }

        public Prices GetCalculatedPrices()
        {
            return prices;
        }

        public void Produce()
        {
            double sell = 0, buy = 0;

            foreach(Score score in scores.Values)
            {
                double price = prices.GetPrice(score.product_id);
                sell += score.supplies.Sum(x => x.GetSupplyAmount(score.product_id, price) * price);
                buy += score.demands.Sum(x => x.GetDemandAmount(score.product_id, price) * price);
            }

            double sell_rate = sell == 0 ? 1.0 : (buy / sell);
            Lib.Debug($"sell_rate={sell_rate}");

            foreach (Score score in scores.Values)
            {
                double price = prices.GetPrice(score.product_id);
                score.supplies.ForEach(x => ((IGetOwner)x).ReceiveWithPatent(x.GetSupplyAmount(score.product_id, price) * price * sell_rate));
            }
            foreach (Score score in scores.Values)
            {
                double price = prices.GetPrice(score.product_id);
                score.demands.ForEach(x => ((IGetOwner)x).GetOwner().Pay(x.GetDemandAmount(score.product_id, price) * price));
            }
        }
    }

    public enum CalculatorState
    {
        Stop,
        Calculating,
    }
}

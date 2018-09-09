using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class MEnv
    {
        //Date
        public static DateTime init_date = new DateTime(2018, 1, 1);

        //Rand
        public const bool is_reset_seed = false;

        //Map
        public const int x_size = 32;
        public const int y_size = 32;
        public const string map_filename = "map.json";

        //Construction
        public const double construction_population = 100000;
        public const double destruction_population = construction_population * 1.5;

        //Bank
        public const double human_cash_rate_base = 0.9;
        public const int loan_base_term = 365 * 2;
        public const double individual_loan_rate = 5.0;

        //Calculator
        public const double humans_consume_rate = 1.0 / 365.0;
        public const double supply_border_rate = 0.9;
        public const double half_output_labor_num = 100;
        public const double patent_rate = 0.10;
        public const double init_distance = 100.0;
        public const double optimize_price_accuracy = 0.00001;
        public const double price_diff_limit = 0.00001;
        public const double amount_diff_limit = 5;

        //Money
        public const double init_bank_money = 10000000;
        public const double init_humans_money = 100.0 * 365.0;
        public const double init_company_money = 10000000;

        //Humans
        public const double init_humans_size = 2000.0;
        public const int increase_num = 20;

        //Company
        public const int default_cpu_num = 5;
        public const int cpu_food_research_rate = 4;
        public const double research_food_profit_rate = 0.8;

        //Reseach
        public const double technology_frequency = 365.0 * 5.0;
        public const double technology_probability = 1.0 / 365.0;
        public const double food_probability = 2.0 / 365.0;
        public const int max_food_count = 9;

        //Save
        public const string version = "1.0";

        //FactoryCounter
        public const int max_factory_count = 400;

        //Skill
        public const double level_up_cost_rate = 0.1;
    }
}

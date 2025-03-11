namespace AidnBackend
{
    public class ScoreService
    {
        public int CalculateScore(int temp, int hr, int rr)
        {

            if (temp < 31 || temp > 42)
            {
                throw new Exception($"Wrong TEMP value, must be between 31 and 42, you've sent: {temp}.");
            }

            if (hr < 25 || hr > 220)
            {
                throw new Exception($"Wrong HR value, must be between 25 and 220, you've sent {hr}.");
            }

            if (rr < 3 || rr > 60)
            {
                throw new Exception($"Wrong RR value, must be between 3 and 60, you've sent {rr}.");
            }


            int score = ResolveTempScore(temp) + ResolveHRScore(hr) + ResolveRRScore(rr);
            Console.WriteLine("Score: " + score);
            return score;
        }

        // 31..35 3
        // 35..36 1
        // 36..38 0
        // 38..39 1
        // 39..42 2
        public int ResolveTempScore(int temp)
        {
            if (temp >= 31 && temp <= 35)
            {
                return 3;
            }
            else if (temp > 35 && temp <= 36 || temp > 38 && temp <= 39)
            {
                return 1;
            }
            else if (temp > 36 && temp <= 38)
            {
                return 0;
            }
            else if (temp > 39 && temp <= 42)
            {
                return 2;
            }

            throw new Exception("Invalid temperature value.");
        }

        // 25..40 3
        // 40..50 1
        // 50..90 0
        // 90..110 1
        // 110..130 2
        // 130..220 3
        public int ResolveHRScore(int hr)
        {
            if (hr >= 25 && hr <= 40 || hr >= 130 && hr <= 220)
            {
                return 3;
            }
            else if (hr > 40 && hr <= 50 || hr > 90 && hr <= 110)
            {
                return 1;
            }
            else if (hr > 50 && hr <= 90)
            {
                return 0;
            }
            else if (hr > 110 && hr <= 130)
            {
                return 2;
            }
            throw new Exception("Invalid heart rate value.");
        }

        // 3..8 3
        // 8..11 1
        // 11..20 0
        // 20..24 2
        // 24..60 3
        public int ResolveRRScore(int rr)
        {
            if (rr >= 3 && rr <= 8 || rr >= 24 && rr <= 60)
            {
                return 3;
            }
            else if (rr > 8 && rr <= 11 || rr > 20 && rr <= 24)
            {
                return 1;
            }
            else if (rr > 11 && rr <= 20)
            {
                return 0;
            }
            throw new Exception("Invalid respiratory rate value.");
        }
    }
}
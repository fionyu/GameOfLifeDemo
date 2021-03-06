﻿namespace GameHost1
{
    public class Cell : ILife
    {
        public int Generation { get; set; } = 0;
        public bool IsAlive { get; set; }

        public Cell[,] Partners { get; set; }

        private int Interval { get; set; }
        private bool IsActive { get; set; } = false;
        private int ActiveTime { get; set; }

        public Cell() {}

        public Cell(bool isAlive, int interval, int activeTime)
        {
            this.IsAlive = isAlive;

            this.Interval = interval;
            this.ActiveTime = activeTime;
        }

        /// <summary>
        /// 1. 每個細胞有兩種狀態 - 存活或死亡，每個細胞與以自身為中心的周圍八格細胞產生互動（如圖，黑色為存活，白色為死亡）
        /// 2. 當前細胞為存活狀態時，當周圍的存活細胞低於2個時（不包含2個），該細胞變成死亡狀態。（模擬生命數量稀少）
        /// 3. 當前細胞為存活狀態時，當周圍有2個或3個存活細胞時，該細胞保持原樣。
        /// 4. 當前細胞為存活狀態時，當周圍有超過3個存活細胞時，該細胞變成死亡狀態。（模擬生命數量過多）
        /// 5. 當前細胞為死亡狀態時，當周圍有3個存活細胞時，該細胞變成存活狀態。（模擬繁殖）
        /// 6. 可以把最初的細胞結構定義為種子，當所有在種子中的細胞同時被以上規則處理後，可以得到第一代細胞圖。按規則繼續處理當前的細胞圖，可以得到下一代的細胞圖，周而復始。 
        /// </summary>
        /// <returns></returns>
        private bool CheckIsAlive()
        {
            var isAlive = this.IsAlive;
            int aliveCount = 0;

            for (int i = 0; i < Partners.GetLength(0); i++)
                for (int k = 0; k < Partners.GetLength(1); k++)
                    if (Partners[i, k].IsAlive) aliveCount++;

            if (this.IsAlive)
            {
                aliveCount--;
                if (aliveCount < 2 || aliveCount > 3)
                    isAlive = false;
            }
            else
            {
                if (aliveCount == 3)
                    isAlive = true;
            }
            return isAlive;
        }

        private void Evolve() 
        {
            if (Partners != null) 
            {
                this.IsAlive = CheckIsAlive();
                Generation++;
            }
        }

        public void PushToNextFrame(int index)
        {
            if (IsActive)
            {
                if ((index - ActiveTime) % Interval == 0)
                    Evolve();
            }
            else
            {
                if (index == ActiveTime)
                    IsActive = true;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace Elections.Diagrams
{
    public class DiagramDataCreator
    {
        public static DiagramData Create(string fileName, string picName, string tittle, Dictionary<string, int> orderNumbers)
        {
            var electionCommitteeResults = new ElectionCommitteeResults(fileName);

            var item = new DiagramData()
            {
                ChartTitle = tittle,
                HorizontalNames = electionCommitteeResults.uiks.ToArray(),
                PicName = picName,
                RowItem = new RowItem[orderNumbers.Count],
            };

            foreach (var participantData in electionCommitteeResults.partiesData)
            {
                var participantName = participantData.Key;

                if (orderNumbers.ContainsKey(participantName))
                {
                    var index = orderNumbers[participantName] - 1;
                    item.RowItem[index] = new RowItem()
                    {
                        Name = $"{participantName}, {participantData.Value.Percent.ToString().Replace(",", ".")}%",
                        Values = participantData.Value.LocalElectionCommittees.Select(v => v.Percent / 100).ToList()
                    };
                }
            }

            return item;
        }
    }
}

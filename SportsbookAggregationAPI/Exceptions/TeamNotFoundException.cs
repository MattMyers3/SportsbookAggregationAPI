using System;

namespace SportsbookAggregationAPI.Exceptions
{
    class TeamNotFoundException : Exception
    {
        public string Team;
        public TeamNotFoundException(string team)
            : base(team)
        {
            Team = team;
        }

        public TeamNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

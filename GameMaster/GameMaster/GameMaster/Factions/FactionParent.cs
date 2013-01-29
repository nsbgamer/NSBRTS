using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaster.Factions
{
    public abstract class FactionParent
    {
        #region General Class Comments

        /// <summary>  
        /// This is an abstract class that describes the required information for any child class that want to be considered Resources. In order to be a Faction, it must contain the following members and override the following methods. Add anything appropriate in order to make your specific Faction unique.
        /// </summary>  


        /// <param name="name">A private STRING member that will be used to label the faction for applications such as the inventory or other UI classes. It will be provided to the program through the XML description program that will be read into the program. This parameter will be able to be returned to another class, but will not be allowed to be edited by another class.</param>  
        /// <param name="id">The current game's database identification number for the faction.</param>  

        /// <remarks>
        /// I do not currently have a strategy for managing ID numbers for the game yet
        /// This needs to be decided upon and implemented over time
        /// SPRADLIN 11/6/12
        /// </remarks>

        #endregion

        #region Class Members

        private string name;
        private int id;

        #endregion

        #region Accessors

        public string Name { get { return name; } }
        public int ID { get { return id; } }

        #endregion
    }
}

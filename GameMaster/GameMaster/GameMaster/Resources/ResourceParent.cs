using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameMaster.Factions;
using GameMaster.Bases;

namespace GameMaster.Resources
{
    public abstract class ResourceParent
    {
        #region General Class Comments

        /// <summary>  
        /// This is an abstract class that describes the required information for any child class that want to be considered Resources. In order to be a Resource, it must contain the following members and override the following methods. Add anything appropriate in order to make your specific Resource unique.
        /// </summary>  


        /// <param name="resourceName">A private STRING member that will be used to label the resource for applications such as the inventory or other UI classes. It will be provided to the program through the XML description program that will be read into the program. This parameter will be able to be returned to another class, but will not be allowed to be edited by another class.</param>  
        /// <param name="idNum">The current game's database identification number for the resource.</param>  
        /// <param name="type">This will be a resource type described by the Resource namespace's enums. It will be a descriptive string that describes the type of resource that the object is. For current list of ResourceTypes, see ResourceEnums.</param>  
        /// <param name="resourceUnitsPerTurn">An INTEGER that describes the number of units per turn of the resource produced. It will be generated randomly at the resource creation within a range of acceptable units provided in the XML document.</param>  
        /// <param name="resourceMaxTurns">An INTEGER that describes how many turns a resouce will be available for once it is active.</param>  
        /// <param name="currNumTurns">An INTEGER that counts the number of turns that the resource has been active. Once the max number of turns has been reached, the resource will no longer provide resources to the Faction owner. If the Resource is unowned, this number will not increment.</param>  
        /// <param name="resourceBaseExists">A BOOL that will tell whether or not the required structure has been built on the resource in order to make it produce units. If there is no structure on the resource, the resource cannot output any resources.</param>  
        /// <param name="resourceGridLocation">A VECTOR2 that holds the (x,y) location of the resource on the game grid.</param>  
        /// <param name="resourceIsAccessible">A BOOL that will be true only when the resource has a structure built on it and is also connected to a base that is owned by a faction by a road. If the value is false, the resource will not produce any resource units.</param>  
        /// <param name="resourceFactionOwner">A FACTIONPARENT object that is the object whose inventory will receive produced resources each turn.</param>  
        /// <param name="resourceClosestBase">A BASEPARENT object that is the closest base connected to the resource. The resource belongs to the current faction that controls the closest base.</param>  


        /// <remarks>
        /// I do not currently have a strategy for managing ID numbers for the game yet
        /// This needs to be decided upon and implemented over time
        /// SPRADLIN 11/6/12
        /// </remarks>

        #endregion

        #region Required Class Members

        private string resourceName;
        private int idNum;
        private ResourceType type; 
        private int resourceUnitsPerTurn;
        private int resourceMaxTurns;
        private int currNumTurns;
        private bool resourceBaseExists;
        private Vector2 resourceGridLocation;
        private bool resourceIsAccessible;
        private FactionParent resourceFactionOwner;
        private BaseParent resourceClosestBase;

        #endregion

    }
}


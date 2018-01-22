﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using the Marvin template for generating inheritance and model merge.
// If you have any questions or suggestions for improvement regarding this code, contact Thomas Fuchs. I allways need feedback to improve.
//
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated. So even when you think you can do better,
// don't touch it.
// -----------------------------------------------------------------------------
using System;
using System.Linq;
using Marvin.Model;
using System.Collections.Generic;
using Marvin.TestTools.Test.Model;

namespace Marvin.TestTools.TestMerge.Model
{
    public partial interface IEnhancedChildRepository 
    {
        /// <summary>
        /// Creates instance with all not nullable properties prefilled including merged properties
        /// </summary>
        EnhancedChild Create(double blub, Int32 number); 
        /// <summary>
        /// Creates instance with all not nullable properties prefilled including merged properties
        /// </summary>
        EnhancedChild Create(double blub, TopParent parent); 
        /// <summary>
        /// Method inherited from the <see cref="ITopParentRepository"/>.
        /// For details have a look at <see cref="ITopParentRepository.GetAllByUpdated"/>
        /// documentation.
        /// </summary>
        IEnumerable<EnhancedChild> GetAllByUpdated(System.DateTime updated);
        /// <summary>
        /// Method inherited from the <see cref="ITopParentRepository"/>.
        /// For details have a look at <see cref="ITopParentRepository.GetByNumber"/>
        /// documentation.
        /// </summary>
        EnhancedChild GetByNumber(System.Int32 number);
        /// <summary>
        /// Method inherited from the <see cref="ITopParentRepository"/>.
        /// For details have a look at <see cref="ITopParentRepository.GetCreatedAndUpdated"/>
        /// documentation.
        /// </summary>
        EnhancedChild GetCreatedAndUpdated(System.DateTime created, System.DateTime updated);
        /// <summary>
        /// Method inherited from the <see cref="ITopParentRepository"/>.
        /// For details have a look at <see cref="ITopParentRepository.GetUpdatedAndNumber"/>
        /// documentation.
        /// </summary>
        IEnumerable<EnhancedChild> GetUpdatedAndNumber(System.DateTime updated, System.Int32 number);
    }

    internal partial class EnhancedChildRepository
    {
        public EnhancedChild Create(double blub, Int32 number) 
        {
            var entity = Create();
            entity.MergeParent.Number = number;
            entity.Blub = blub;
            return entity;
        }
        public EnhancedChild Create(double blub, TopParent parent) 
        {
            var entity = base.Create();
            entity.Id = parent.Id;
            entity.MergeParent = parent;
            entity.Blub = blub;
            return entity;
        }
        public IEnumerable<EnhancedChild> GetAllByUpdated(System.DateTime updated)
        {
            var result = ParentUow.GetRepository<ITopParentRepository>().GetAllByUpdated(updated);
            if (result == null || !result.Any())
				return new List<EnhancedChild>();
            return result.Select(LoadByTopParent).Where(entity => entity != null);
        }
        public EnhancedChild GetByNumber(System.Int32 number)
        {
            var result = ParentUow.GetRepository<ITopParentRepository>().GetByNumber(number);
            if (result == null)
				return null;
            return LoadByTopParent(result);
        }
        public EnhancedChild GetCreatedAndUpdated(System.DateTime created, System.DateTime updated)
        {
            var result = ParentUow.GetRepository<ITopParentRepository>().GetCreatedAndUpdated(created, updated);
            if (result == null)
				return null;
            return LoadByTopParent(result);
        }
        public IEnumerable<EnhancedChild> GetUpdatedAndNumber(System.DateTime updated, System.Int32 number)
        {
            var result = ParentUow.GetRepository<ITopParentRepository>().GetUpdatedAndNumber(updated, number);
            if (result == null || !result.Any())
				return new List<EnhancedChild>();
            return result.Select(LoadByTopParent).Where(entity => entity != null);
        }
    }
}
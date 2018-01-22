﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using the Marvin template for generating Repositories and a Unit of Work for Entity Framework.
// If you have any questions or suggestions for improvement regarding this code, contact Thomas Fuchs. I allways need feedback to improve.
//
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated. So even when you think you can do better,
// don't touch it.
//------------------------------------------------------------------------------
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Marvin.TestTools.Test.Model;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using Marvin.Model;

namespace Marvin.TestTools.TestMerge.Model
{
    internal partial class MergedChildTPT2Repository : InheritedEntityFrameworkRepository<MergedChildTPT2>, IMergedChildTPT2Repository
    {
        public static IRepository Create(IUnitOfWork uow, DbContext context, IUnitOfWork parentUow)
        {
            var instance = new MergedChildTPT2Repository();
            instance.UnitOfWork = uow;
            instance.Context = context;
            instance.ParentUow = parentUow;
            instance.DbSet = context.Set<MergedChildTPT2>();
            return instance;
        }

        public MergedChildTPT2 LoadByTopParent(TopParent parent)
        {
            var mergedChildTPT2 = DbSet.FirstOrDefault(e => e.Id == parent.Id);
            if (mergedChildTPT2 != null)
                mergedChildTPT2.MergeParent = parent;
            return mergedChildTPT2;               
        }

        public MergedChildTPT2 LoadTopParentProperties(MergedChildTPT2 child)
        {
            child.MergeParent = ParentUow.GetRepository<ITopParentRepository>().GetByKey(child.Id);
            return child;               
        }
 
        public override MergedChildTPT2 Create()
        {
            var newInstance = base.Create();
            newInstance.MergeParent = ParentUow.GetRepository<ITopParentRepository>().Create();
            EntityIdListener.Listen(newInstance.MergeParent, newInstance);           
            return newInstance;  
        }

        public override MergedChildTPT2 GetByKey(long id)
        {
		    var result = DbSet.FirstOrDefault(e => e.Id == id);
            if (result == null)
                return result;
            result.MergeParent = ParentUow.GetRepository<ITopParentRepository>().GetByKey(result.Id);
            return result;
        }

        public override ICollection<MergedChildTPT2> GetByKeys(long[] ids)
        {
			var result= DbSet.Where(e => ids.Contains(e.Id)).ToDictionary(e => e.Id, e => e);
			var entityIds = result.Keys.ToArray();
			var parents = ParentUow.GetRepository<ITopParentRepository>().GetByKeys(entityIds).ToDictionary(e => e.Id, e => e);
			foreach(var id in entityIds)
			{
				result[id].MergeParent = parents[id];
			}
            return result.Values;
        }

        public ICollection<MergedChildTPT2> GetAll(bool deleted)
		{
			var result= DbSet.Where(e => e.Deleted == null || deleted).ToDictionary(e => e.Id, e => e);
			var entityIds = result.Keys.ToArray();
			var parents = ParentUow.GetRepository<ITopParentRepository>().GetByKeys(entityIds).ToDictionary(e => e.Id, e => e);
			foreach(var id in entityIds)
			{
				result[id].MergeParent = parents[id];
			}
            return result.Values;
		}
        public virtual MergedChildTPT2 GetByTemp(double temp)
        {
		    var result = DbSet.FirstOrDefault(e => e.Temp == temp && e.Deleted == null);
            if (result == null)
                return result;
            result.MergeParent = ParentUow.GetRepository<ITopParentRepository>().GetByKey(result.Id);
            return result;
        }

        public virtual MergedChildTPT2 GetByB(int b)
        {
		    var result = DbSet.FirstOrDefault(e => e.B == b && e.Deleted == null);
            if (result == null)
                return result;
            result.MergeParent = ParentUow.GetRepository<ITopParentRepository>().GetByKey(result.Id);
            return result;
        }

        public IEnumerable<MergedChildTPT2> GetCombinedTPTInheritance(global::System.Nullable<int> combinedBaseTPT)
        {
			var result= DbSet.Where(e => e.CombinedBaseTPT == combinedBaseTPT).ToDictionary(e => e.Id, e => e);
			var entityIds = result.Keys.ToArray();
			var parents = ParentUow.GetRepository<ITopParentRepository>().GetByKeys(entityIds).ToDictionary(e => e.Id, e => e);
			foreach(var id in entityIds)
			{
				result[id].MergeParent = parents[id];
			}
            return result.Values;
        }

        public IEnumerable<MergedChildTPT2> GetCombiTPT(global::System.Nullable<int> combi1TPT, global::System.Nullable<int> combi2TPT)
        {
			var result= DbSet.Where(e => e.Combi1TPT == combi1TPT && e.Combi2TPT == combi2TPT).ToDictionary(e => e.Id, e => e);
			var entityIds = result.Keys.ToArray();
			var parents = ParentUow.GetRepository<ITopParentRepository>().GetByKeys(entityIds).ToDictionary(e => e.Id, e => e);
			foreach(var id in entityIds)
			{
				result[id].MergeParent = parents[id];
			}
            return result.Values;
        }

        protected override void ExecuteRemove(MergedChildTPT2 entity, bool permanent)
        {
            if (permanent)
                base.ExecuteRemove(entity, permanent);
            else
                entity.Deleted = DateTime.Now;
		    ParentUow.GetRepository<ITopParentRepository>().Remove(entity, permanent);
		}

		protected override void ExecuteRemoveRange(IEnumerable<MergedChildTPT2> entities, bool permanent)
		{
            if (permanent)
            {
                base.ExecuteRemoveRange(entities, permanent);
            }
            else
            {
                foreach (var entity in entities)
                {
                    entity.Deleted = DateTime.Now;
                }
            }
		    ParentUow.GetRepository<ITopParentRepository>().RemoveRange(entities.Select(e => e.MergeParent), permanent);
		}

    }
}
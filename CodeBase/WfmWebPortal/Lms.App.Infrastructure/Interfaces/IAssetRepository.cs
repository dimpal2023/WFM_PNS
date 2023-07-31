using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;

namespace Wfm.App.Infrastructure.Interfaces
{
    public interface IAssetRepository
   {
         AssetMappingMasterMetaData Find(Guid asset_id);
         void Create(AssetMappingMasterMetaData asset);
         void Update(AssetMappingMasterMetaData asset);
        List<AssetMappingMasterMetaData> GetAllAsset();
        List<AssetMasterMetaData> GetAssetByDepartmentId(Guid department_id, Guid sub_dept_id);
        void Delete(Guid asset_id);
    }
}

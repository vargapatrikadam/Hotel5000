﻿using Core.Entities.LodgingEntities;
using Core.Enums.Lodging;
using Core.Helpers.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.LodgingDomain.LodgingManagementService
{
    public interface ILodgingService
    {
        Task<Result<IReadOnlyList<Lodging>>> GetLodging(int? id = null,
            string name = null,
            string lodgingType = null,
            DateTime? reservableFrom = null,
            DateTime? reservableTo = null,
            string address = null,
            string owner = null,
            int? skip = null,
            int? take = null);

        Task<Result<bool>> UpdateLodging(Lodging newLodging,
            int oldLodgingId,
            int resourceAccessorId);

        Task<Result<bool>> RemoveLodging(int lodgingId,
            int resourceAccessorId);

        Task<Result<bool>> AddLodging(Lodging lodging, string lodgingType, int resourceAccessorId);
    }
}

﻿using BL.Model.Wallet;
using SpendingTracker.Models.Wallet.Request;
using SpendingTracker.Models.Wallet.Response;
using System.Collections.Generic;
using System.Linq;

namespace SpendingTracker.Mappers
{
    public static class WalletMapper
    {
        public static WalletResponse ToResponse(this WalletDomain domain) => new WalletResponse
        {
            Id = domain.Id,
            Name = domain.Name
        };

        public static IEnumerable<WalletResponse> AllToResponse(
            this IEnumerable<WalletDomain> domains) => domains
                .Select(d => d.ToResponse())
                .ToList();

        public static AddWalletDto ToDto(this AddWalletRequest request, int userId) => new AddWalletDto
        {
            Name = request.Name,
            Categories = request.Categories,
            UserId = userId
        };
    }
}
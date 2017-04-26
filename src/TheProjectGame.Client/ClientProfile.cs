using System;
using AutoMapper;
using TheProjectGame.Contracts.Messages.Structures;
using TheProjectGame.Game;

namespace TheProjectGame.Client
{
    public class ClientProfile : Profile
    {
        public ClientProfile(bool useCurrentTimestamp)
        {
            CreateMap<Position, Location>();

            CreateMap<TaskTile, TaskField>()
                .ForMember(t => t.PlayerIdSpecified, t => t.MapFrom(b => b.Player != null))
                .ForMember(t => t.PlayerId, t => t.MapFrom(b => b.Player == null ? 0 : b.Player.Id))
                .ForMember(t => t.PieceIdSpecified, t => t.MapFrom(b => b.Piece != null))
                .ForMember(t => t.PieceId, t => t.MapFrom(b => b.Piece == null ? 0 : b.Piece.Id))
                .WithCurrentTimestamp(useCurrentTimestamp);

            CreateMap<GoalTile, GoalField>()
                .ForMember(t => t.PlayerIdSpecified, t => t.MapFrom(b => b.Player != null))
                .ForMember(t => t.PlayerId, t => t.MapFrom(b => b.Player == null ? 0 : b.Player.Id))
                .WithCurrentTimestamp(useCurrentTimestamp);
        
            CreateMap<BoardPiece, Piece>()
                .ForMember(t => t.PlayerIdSpecified, t => t.MapFrom(b => b.Player != null))
                .ForMember(t => t.PlayerId, t => t.MapFrom(b => b.Player == null ? 0 : b.Player.Id))
                .WithCurrentTimestamp(useCurrentTimestamp);

            CreateMap<GamePlayer, Player>()
                .ForMember(p => p.Type, t => t.MapFrom(p => p.Role));
        }
    }

    static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDestination> WithCurrentTimestamp<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression, 
            bool useCurrentTimestamp)
            where TDestination: ITimestamped
        {
            if (useCurrentTimestamp)
            {
                mappingExpression.ForMember(t => t.Timestamp, t => t.Ignore());
                mappingExpression.AfterMap((src, dest) => dest.Timestamp = DateTime.Now);
            }
            return mappingExpression;
        }
    }
}

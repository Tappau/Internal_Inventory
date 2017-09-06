using System.Collections.Generic;
using ComicsInventory.DAL.DTOs;
using ComicsInventory.DAL.Entities;

namespace ComicsInventory.DAL.Repositories.Interfaces
{
    public interface IBoxRepo
    {
        /// <summary>
        ///     Retires the specified box identifier.
        /// </summary>
        /// <param name="boxId">The box identifier.</param>
        void Retire(int boxId);

        /// <summary>
        ///     Gets highest BoxID and adds one to work out what the next box number will be
        ///     This is purely for displaying of information during creation of a new box
        /// </summary>
        /// <returns></returns>
        int NextBoxId();

        /// <summary>
        ///     Retrives Select group of data related to given Box ID
        /// </summary>
        /// <param name="boxId">The box identifier.</param>
        /// <returns></returns>
        IEnumerable<BoxContentDto> GetBoxContents(int boxId);

        string ConnectString { get; }
        IEnumerable<BoxStore> SelectAll();
        BoxStore GetById(int id);
        void Insert(BoxStore entity);
        void InsertAndSubmit(BoxStore entity);
        void Update(BoxStore entity);
        void UpdateAndSubmit(BoxStore entity);
        void Delete(int id);
        void Save();
    }
}
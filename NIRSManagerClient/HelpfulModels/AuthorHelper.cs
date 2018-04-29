using NIRSCore;
using NIRSCore.DataBaseModels;

namespace NIRSManagerClient.HelpfulModels
{
    public sealed class AuthorHelper
    {
        /// <summary>
        /// Идентификатор автора
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// ФИО Автора
        /// </summary>
        public string Fio { get; private set; }

        /// <summary>
        /// Должность
        /// </summary>
        public string Position { get; private set; }

        /// <summary>
        /// Название организации
        /// </summary>
        public string Organization { get; private set; }

        /// <summary>
        /// Путь к личному фото
        /// </summary>
        public string PathPhoto { get; private set; }


        public AuthorHelper(int id)
        {
            UserId = id;
            Author authorQuery = NirsSystem.GetAuthor(id);
            Fio = authorQuery.SurName + " " + authorQuery.Name + " " + authorQuery.SecondName;
            PathPhoto = authorQuery.PathPhoto;
            Position = NirsSystem.GetPosition(authorQuery.PositionId)?.PositionName;
            Organization = NirsSystem.GetOrganization(authorQuery.OrganizationId)?.OrganizationName;
        }
    }
}

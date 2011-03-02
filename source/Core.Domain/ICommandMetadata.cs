using System;

namespace Core.Domain
{
    public interface ICommandMetadata
    {
        /// <summary>
        /// Unique Id of Command
        /// </summary>
        String CommandId { get; set; }

        /// <summary>
        /// User Id of user who initiate this command
        /// </summary>
        String UserId { get; set; }

        /// <summary>
        /// Assembly qualified CLR Type name of Command Type
        /// </summary>
        String TypeName { get; set; }

        /// <summary>
        /// Time when command was created
        /// </summary>
        DateTime CreatedDate { get; set; }
    }
}
using TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

namespace TTVMAMobileWebMiddleware.Domain.Requests
{
    public class CitizenWithDetailsRequest
    {
        /// <summary>
        /// The citizen.
        /// </summary>
        public required Citizen Citizen { get; set; }
        /// <summary>
        /// The address.
        /// </summary>
        public required List<CitizenAddress> Address { get; set; }

        /// <summary>
        /// List of identity documents related to the citizen.
        /// </summary>
        public required List<CitizenIdentityDocument> Document { get; set; }

        public required List<CitizenSignature> Signatures { get; set; }

        public required List<CitizenFaceImage> FaceImages { get; set; }

    }
}

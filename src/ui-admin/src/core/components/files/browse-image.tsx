import { useState } from "react";
import RenderIf from "../../render-if";

export default function BrowseImage({
  label = "Select Image",
  onSelected,
  onCleared,
}: Readonly<{
  label: string;
  onSelected: (file: File) => void;
  onCleared: () => void;
}>) {
  const [imagePreviewUrl, setImagePreviewUrl] = useState(null);
  const [selectedImage, setSelectedImage] = useState<File | null>(null);

  function onImageSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      // Validate file type
      const validImageTypes = [
        "image/jpeg",
        "image/png",
        "image/gif",
        "image/webp",
      ];
      if (!validImageTypes.includes(file.type)) {
        alert("Please select a valid image file (JPEG, PNG, GIF, or WebP)");
        return;
      }

      // Validate file size (5MB max)
      const maxSizeInBytes = 5 * 1024 * 1024;
      if (file.size > maxSizeInBytes) {
        alert("Image file size must not exceed 5MB");
        return;
      }

      setSelectedImage(file);

      // Create preview
      const reader = new FileReader();
      reader.onload = (e: any) => {
        setImagePreviewUrl(e.target.result);
      };
      reader.readAsDataURL(file);

      onSelected(file);
    }
  }

  function removeImage() {
    setSelectedImage(null);
    setImagePreviewUrl(null);
    onCleared();
  }

  return (
    <div className="mb-3">
      <label htmlFor="productImage" className="form-label">
        Product Image
      </label>

      <div className="card bg-light">
        <div className="card-body">
          <div className="mb-3">
            <input
              type="file"
              className="form-control"
              id="productImage"
              accept="image/*"
              onChange={onImageSelected}
            />
            <small className="text-muted d-block mt-2">
              Supported formats: JPEG, PNG, GIF, WebP. Max size: 5MB
            </small>
          </div>

          <RenderIf flag={imagePreviewUrl} className="mt-3">
            <div className="d-flex justify-content-between align-items-start">
              <div>
                <label className="form-label">Preview:</label>
                <img
                  src={imagePreviewUrl!}
                  alt="Preview"
                  className="img-thumbnail"
                  style={{ maxWidth: "200px", maxHeight: "200px" }}
                />
              </div>
              <button
                type="button"
                className="btn btn-sm btn-outline-danger"
                onClick={removeImage}
              >
                Remove
              </button>
            </div>
          </RenderIf>

          <RenderIf flag={selectedImage && !imagePreviewUrl} className="mt-3">
            <small className="text-success">
              <i className="bi bi-check-circle"></i>
              {selectedImage?.name} selected
            </small>
          </RenderIf>
        </div>
      </div>
    </div>
  );
}

// import { classes } from "./create-product.css";

import { useState } from "react";
import SelectCategoryControl from "../category-select-control/select-category-control";
import RenderIf from "../../../core/render-if";

export default function CreateProduct() {
  const [imagePreviewUrl, setImagePreviewUrl] = useState(null);
  const [selectedImage, setSelectedImage] = useState<File | null>(null);
  const [propertiesArray, setPropertiesArray] = useState<[]>([]);
  const [form, setForm] = useState();

  function onSubmit() {}

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
    }
  }

  function removeImage() {
    setSelectedImage(null);
    setImagePreviewUrl(null);
  }

  function resetForm() {
    //  createForm.reset();
    setPropertiesArray([]);
    setSelectedImage(null);
    setImagePreviewUrl(null);
  }

  return (
    <>
      <div className="row justify-content-center">
        <div className="col-md-8">
          <div className="card shadow-lg">
            <div className="card-header bg-primary text-white">
              <h2 className="mb-0">Create New Product</h2>
            </div>

            <div className="card-body p-4">
              <form onSubmit={onSubmit}>
                <SelectCategoryControl />
              </form>

              <div className="mb-3">
                <label htmlFor="name" className="form-label">
                  Product Name
                </label>

                <input
                  type="text"
                  className="form-control"
                  id="name"
                  placeholder="Enter product name"
                />
              </div>

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

                    <RenderIf
                      flag={selectedImage && !imagePreviewUrl}
                      className="mt-3"
                    >
                      <small className="text-success">
                        <i className="bi bi-check-circle"></i>
                        {selectedImage?.name} selected
                      </small>
                    </RenderIf>
                  </div>
                </div>
              </div>

              <div className="mb-3">
                <label htmlFor="inStockCount" className="form-label">
                  In Stock Count
                </label>

                <input
                  type="text"
                  className="form-control"
                  id="inStockCount"
                  placeholder="Enter in Stock Count"
                />
              </div>

              <div className="row">
                <div className="col-md-4 mb-3">
                  <label htmlFor="price" className="form-label">
                    Unit Price
                  </label>

                  <div className="input-group">
                    <span className="input-group-text">$</span>
                    <input
                      type="number"
                      className="form-control"
                      id="price"
                      placeholder="0.00"
                      min="0"
                      step="0.01"
                    />
                  </div>
                </div>

                <div className="col-md-4 mb-3">
                  <label htmlFor="price" className="form-label">
                    Minimum Price
                  </label>

                  <div className="input-group">
                    <span className="input-group-text">$</span>
                    <input
                      type="number"
                      className="form-control"
                      id="minimumPrice"
                      placeholder="0.00"
                      min="0"
                      step="0.01"
                    />
                  </div>
                </div>

                <div className="col-md-4 mb-3">
                  <label htmlFor="price" className="form-label">
                    Maximum Price
                  </label>

                  <div className="input-group">
                    <span className="input-group-text">$</span>
                    <input
                      type="number"
                      className="form-control"
                      id="maximumPrice"
                      placeholder="0.00"
                      min="0"
                      step="0.01"
                    />
                  </div>
                </div>
              </div>

              <div className="card mt-4 mb-4">
                <div className="card-header bg-secondary">
                  <h5 className="mb-0 text-white">Product Properties</h5>
                </div>
                <div className="card-body">
                  <RenderIf flag={propertiesArray.length === 0}>
                    <div className="text-muted">
                      No properties added yet. Click "Add Property" to get
                      started.
                    </div>
                  </RenderIf>
                </div>
              </div>

              <div className="d-flex gap-2 justify-content-end">
                <button
                  type="button"
                  className="btn btn-light"
                  onClick={resetForm}
                >
                  Reset
                </button>
                <button type="submit" className="btn btn-primary">
                  <span>Create Product</span>
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

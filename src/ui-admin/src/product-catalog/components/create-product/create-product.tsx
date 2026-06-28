// import { classes } from "./create-product.css";

import { useState } from "react";
import SelectCategoryControl from "../category-select-control/select-category-control";
import BrowseImage from "../../../core/components/files/browse-image";
import type { ProductCreateRequest } from "../../models/product-create.model";
import KeyValuePairsForm from "../../../core/components/forms/key-value-pairs-form";
import catalogServices from "../../services/catalog.services";
import { useNavigate } from "react-router-dom";

export default function CreateProduct() {
  // const [propertiesArray, setPropertiesArray] = useState<[]>([]);
  const [selectedImage, setSelectedImage] = useState<File | null>(null);
  const [form, setForm] = useState<ProductCreateRequest | null>(null);

  const navigator = useNavigate();

  function onSubmit() {}

  function resetForm() {
    setForm(null);
  }

  function createProduct() {
    const formValue = form!;

    const productRequest: ProductCreateRequest = {
      categoryId: formValue.categoryId,
      name: formValue.name,
      inStockCount: formValue.inStockCount,
      price: formValue.price,
      minimumPrice: formValue.minimumPrice,
      maximumPrice: formValue.maximumPrice,
      properties: formValue.properties,
    };

    catalogServices.createProduct(productRequest, selectedImage!).subscribe({
      next: () => {
        navigator("/catalog/products");
      },
      error: (err) => {
        console.log(err);
      },
    });
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
                <SelectCategoryControl
                  onSelected={(id) => setForm({ ...form!, categoryId: id })}
                />
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
                  onChange={(e) => setForm({ ...form!, name: e.target.value })}
                />
              </div>

              <BrowseImage
                label="Product Image"
                onSelected={setSelectedImage}
                onCleared={() => setSelectedImage(null)}
              />

              <div className="mb-3">
                <label htmlFor="inStockCount" className="form-label">
                  In Stock Count
                </label>

                <input
                  type="text"
                  className="form-control"
                  id="inStockCount"
                  placeholder="Enter in Stock Count"
                  onChange={(e) =>
                    setForm({
                      ...form!,
                      inStockCount: parseInt(e.target.value),
                    })
                  }
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
                      onChange={(e) =>
                        setForm({
                          ...form!,
                          price: parseFloat(e.target.value),
                        })
                      }
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
                      onChange={(e) =>
                        setForm({
                          ...form!,
                          minimumPrice: parseFloat(e.target.value),
                        })
                      }
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
                      onChange={(e) =>
                        setForm({
                          ...form!,
                          maximumPrice: parseFloat(e.target.value),
                        })
                      }
                    />
                  </div>
                </div>
              </div>

              <div className="card mt-4 mb-4">
                <div className="card-header bg-secondary">
                  <h5 className="mb-0 text-white">Product Properties</h5>
                </div>
                <KeyValuePairsForm
                  emptyMessage={
                    'No properties added yet. Click "Add Property" to get started.'
                  }
                  onChange={(props) =>
                    setForm({
                      ...form!,
                      properties: props,
                    })
                  }
                />
              </div>

              <div className="d-flex gap-2 justify-content-end">
                <button
                  type="button"
                  className="btn btn-light"
                  onClick={resetForm}
                >
                  Reset
                </button>
                <button
                  type="submit"
                  className="btn btn-primary"
                  onClick={createProduct}
                >
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

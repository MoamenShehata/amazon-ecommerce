// import { classes } from "./create-product.css";

import SelectCategoryControl from "../category-select-control/select-category-control";

export default function CreateProduct() {
  function onSubmit() {}

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
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

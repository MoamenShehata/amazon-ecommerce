import { useState } from "react";
import RenderIf from "../../core/render-if";
import type { CategoryForListModel } from "../models/category-for-list.models";
import CategorySelect from "./category-select-control/category-select";

export interface CategoryCreateModel {
  categoryName: string;
  parentCategoryId: string | null;
}

export default function CreateCategoryForm({
  categories,
  onFormChange,
}: Readonly<{
  categories: CategoryForListModel[];
  onFormChange: (value: CategoryCreateModel) => void;
}>) {
  const [value, setValue] = useState<CategoryCreateModel | null>({
    categoryName: "",
    parentCategoryId: categories[0].id,
  });

  function updateFormValue(value: CategoryCreateModel) {
    setValue({
      categoryName: value.categoryName,
      parentCategoryId: value.parentCategoryId!,
    });

    onFormChange(value);
  }

  return (
    <>
      <form>
        <div className="mb-3">
          <label htmlFor="categoryName" className="form-label">
            Category Name
          </label>

          <input
            type="text"
            id="categoryName"
            className="form-control"
            value={value?.categoryName}
            onChange={(e) =>
              updateFormValue({
                categoryName: e.target.value,
                parentCategoryId: value?.parentCategoryId!,
              })
            }
          />

          <RenderIf flag={false}>
            Category name is required and must be at least 3 characters.
          </RenderIf>
        </div>

        <div className="mb-3">
          <label htmlFor="parentCategoryId" className="form-label">
            Parent Category
          </label>
          <CategorySelect categories={categories} />
        </div>
      </form>
    </>
  );
}

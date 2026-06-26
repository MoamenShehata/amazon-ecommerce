import { useEffect, useState } from "react";
import type { CategoryForListModel } from "../../models/category-for-list.models";

import RenderIf from "../../../core/render-if";
import Modal from "../../../core/bootstrap/components/modal";
import CategorySelect from "./category-select";
import CreateCategoryForm, {
  type CategoryCreateModel,
} from "../create-category-form";

export default function SelectCategoryControl() {
  const [categories, setCategories] = useState<CategoryForListModel[]>([]);
  const [isCategoryModalOpen, setIsCategoryModalOpen] = useState(false);

  const [formValue, setFormValue] = useState<CategoryCreateModel | null>(null);

  function openCategoryModal() {
    setIsCategoryModalOpen(true);
  }

  function closeCategoryModal() {
    setIsCategoryModalOpen(false);
  }

  function createNewCategory() {
    if (formValue == null) {
      alert("Invalid form");
      return;
    }

    catalogServices
      .createCategory({
        name: formValue.categoryName,
        parentCategoryId: formValue.parentCategoryId,
      })
      .subscribe({
        next: (createdCategory) => {
          setCategories([...categories, createdCategory]);
          closeCategoryModal();
        },
        error: (err) => {
          alert(err);
          console.log(err);
        },
      });

    closeCategoryModal();
  }

  useEffect(() => {
    catalogServices
      .getCategoriesPage({
        pageNumber: 1,
        pageSize: 100,
        lastSeenValue: null,
      })
      .subscribe((page) => {
        setCategories(page.items);
      });
  });

  return (
    <>
      <RenderIf flag={isCategoryModalOpen}>
        <h5>
          <Modal
            header="Create New Category"
            isSubmitDisabled={
              formValue == null ||
              formValue.categoryName == null ||
              formValue.categoryName == ""
            }
            onClosed={closeCategoryModal}
            onSubmitted={() => createNewCategory()}
          >
            <CreateCategoryForm
              categories={categories}
              onFormChange={setFormValue}
            />
          </Modal>
        </h5>
      </RenderIf>

      <div className="mb-3">
        <label htmlFor="categoryId" className="form-label">
          Category
        </label>
        <div className="input-group">
          <CategorySelect categories={categories} />

          <button
            type="button"
            className="btn btn-outline-secondary"
            onClick={openCategoryModal}
          >
            New Category
          </button>
        </div>
      </div>
    </>
  );
}

import catalogServices from "../../services/catalog.services";

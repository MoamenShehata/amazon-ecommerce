import type { CategoryForListModel } from "../../models/category-for-list.models";

export default function CategorySelect({
  categories,
}: Readonly<{ categories: CategoryForListModel[] }>) {
  return (
    <>
      <select className="form-control" id="categoryId">
        <option value="">Select a category</option>
        {categories.map((c) => (
          <option key={c.id} value={c.id}>
            {c.name}
          </option>
        ))}
      </select>
    </>
  );
}

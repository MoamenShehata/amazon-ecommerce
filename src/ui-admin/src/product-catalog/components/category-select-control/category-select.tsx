import type { CategoryForListModel } from "../../models/category-for-list.models";

export default function CategorySelect({
  categories,
  onSelected,
}: Readonly<{
  categories: CategoryForListModel[];
  onSelected: (id: string) => void;
}>) {
  return (
    <>
      <select
        className="form-select"
        id="categoryId"
        onChange={(e) => onSelected(e.target.value)}
      >
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

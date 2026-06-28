import { useState } from "react";
import RenderIf from "../../render-if";

export default function KeyValuePairsForm({
  emptyMessage,
  onChange,
}: Readonly<{
  emptyMessage: string;
  onChange: (items: { name: string; value: string }[]) => void;
}>) {
  const [propertiesArray, setPropertiesArray] = useState<
    { name: string; value: string }[]
  >([]);

  function addProperty(): void {
    updatePropeties([...propertiesArray, { name: "", value: "" }]);
  }

  function removeProperty(index: number): void {
    setPropertiesArray(propertiesArray.filter((x, i) => i != index));
  }

  function updatePropeties(items: { name: string; value: string }[]) {
    setPropertiesArray(items);

    onChange(propertiesArray);
  }

  function updatePropertyName(index: number, name: string) {
    updatePropeties(
      propertiesArray.map((x, i) => {
        if (i == index) x.name = name;

        return x;
      }),
    );
  }

  function updatePropertyValue(index: number, value: string) {
    setPropertiesArray(
      propertiesArray.map((x, i) => {
        if (i == index) x.value = value;

        return x;
      }),
    );
  }

  return (
    <div className="card-body">
      <RenderIf flag={propertiesArray.length === 0}>
        <div className="text-muted">{emptyMessage}</div>
      </RenderIf>

      {propertiesArray.map((p, i) => {
        return (
          <div key={p}>
            <div className="row mb-3">
              <div className="col-md-5">
                <input
                  type="text"
                  className="form-control"
                  placeholder="Property Name"
                  onChange={(e) => updatePropertyName(i, e.target.value)}
                />
              </div>
              <div className="col-md-5">
                <input
                  type="text"
                  className="form-control"
                  placeholder="Property Value"
                  onChange={(e) => updatePropertyValue(i, e.target.value)}
                />
              </div>
              <div className="col-md-2">
                <button
                  type="button"
                  className="btn btn-danger w-100"
                  onClick={() => removeProperty(i)}
                >
                  Remove
                </button>
              </div>
            </div>
          </div>
        );
      })}

      <button
        type="button"
        className="btn btn-outline-secondary"
        onClick={addProperty}
      >
        <i className="bi bi-plus"></i> Add Property
      </button>
    </div>
  );
}

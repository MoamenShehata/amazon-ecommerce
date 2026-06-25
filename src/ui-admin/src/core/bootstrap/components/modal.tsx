import type React from "react";
import RenderIf from "../../render-if";

export default function Modal({
  header,
  children,
  isSubmitDisabled,
  onClosed,
  onSubmitted,
}: Readonly<{
  header: string;
  children: React.ReactNode;
  isSubmitDisabled: boolean;
  onClosed: () => void;
  onSubmitted: () => void;
}>) {
  return (
    <>
      <div className="modal-backdrop fade show"></div>

      <div
        className="modal fade show d-block"
        tabIndex={-1}
        role="dialog"
        aria-modal="true"
      >
        <div className="modal-dialog">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">{header}</h5>
              <button
                type="button"
                className="btn-close"
                aria-label="Close"
                onClick={onClosed}
              ></button>
            </div>
            <div className="modal-body">{children}</div>

            <div className="modal-footer">
              <button
                type="button"
                className="btn btn-secondary"
                onClick={onClosed}
              >
                Cancel
              </button>
              <RenderIf flag={isSubmitDisabled}>
                <button
                  type="button"
                  className="btn btn-primary disabled"
                  disabled
                >
                  Save
                </button>
              </RenderIf>

              <RenderIf flag={!isSubmitDisabled}>
                <button
                  type="button"
                  className="btn btn-primary"
                  onClick={onSubmitted}
                >
                  Save
                </button>
              </RenderIf>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

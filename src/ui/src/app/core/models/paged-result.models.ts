export interface PagedResult<TEntity> {
  items: TEntity[];
  totalCount: number;
  lastSeenValue: any;
}

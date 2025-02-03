export class ShopParams {
  brands: string[] = [];
  types: string[] = [];
  sort: string = 'name';
  search: string = '';
  pageNumber: number = 1;
  pageSize: number = 20;
}
export class OrderParams {
  status: string = '';
  pageNumber: number = 1;
  pageSize: number = 20;
}

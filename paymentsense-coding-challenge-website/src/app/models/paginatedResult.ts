import { Country } from './country';

export class PaginatedResult {
  page: number;
  pageSize: number;
  total: number;
  countries: Country[];
}

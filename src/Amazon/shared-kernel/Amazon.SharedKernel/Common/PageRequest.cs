namespace Amazon.SharedKernel.Common;

public record PageRequest(int PageNumber = 1, int PageSize = 30, object LastSeenValue = null);

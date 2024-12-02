namespace UT01325MS3_GYMFEEMANAGEMENT.Repositories.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IMemberRepository Members { get; }
        IPaymentRepository Payments { get; }
        ITrainingProgramRepository TrainingPrograms { get; }
        IAlertRepository Alerts { get; }

        Task<int> CompleteAsync();
    }
}

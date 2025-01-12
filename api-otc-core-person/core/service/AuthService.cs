
using AutoMapper;
using db.Enum;
using Rq = db.dominio;
using Microsoft.EntityFrameworkCore;
using db.Response;
using db.Models;


namespace Core.service
{
    public class AuthService : IAuthService
    {
        private readonly EduDbContext _context;
        private readonly IMapper _mapper;
        private readonly string _active = StatusCode.A.ToString();
        public AuthService(EduDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RsTrxService> validateLogin(Rq.Login model)
        {
            try
            {
                var usuario = await _context.Usuarios.Include(u => u.IdPersonaNavigation).FirstOrDefaultAsync(u =>
                    u.Username == model.User && u.Estado == _active && u.Intento <= 3);
                if (usuario == null)
                {
                    return responseUnauthorized();
                }
                //Ref. Actualizar intentos cuando la informacion esta OK
                if (model.Contrasenia.Equals(usuario.Contrasenia))
                {
                    usuario.Intento = 0;
                    _context.Entry(usuario).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    var user = usuario.IdPersonaNavigation;
                    string nombre = String.Concat(user!.Nombres, " ", user.PrimerApellido, " ", user!.SegundoApellido);
                    return new RsTrxService(StatusCode.Success, usuario.Id, nombre, usuario.Rol);
                }
                else
                {
                    return await HandleInvalidLoginAttempt(usuario);
                }
            }
            catch (Exception ex)
            {
                return new RsTrxService(StatusCode.InternalServerError, 0, ex.Message);
            }
        }

        private async Task<RsTrxService> HandleInvalidLoginAttempt(Usuario usuario)
        {
            usuario.Intento++;

            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return usuario.Intento > 3
                ? responseUnauthorized("Usuario bloqueado.")
                : new RsTrxService(StatusCode.Forbidden, 0, "Usuario/Contraseña incorrecta.");
        }

        public async Task<RsTrxService> rememberPassword(Rq.Login model)
        {
            try
            {
                var resp = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == model.User && u.IdPersonaNavigation!.Cedula == model.Contrasenia);
                return resp == null
                    ? responseUnauthorized()
                    : new RsTrxService(StatusCode.Success, 0, resp.Username);
            }
            catch (Exception ex)
            {
                return new RsTrxService(StatusCode.InternalServerError, 0, ex.Message);
            }
        }

        public async Task<RsTrxService> unlockUser(Rq.Login model)
        {
            try
            {
                var resp = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == model.User && u.IdPersonaNavigation!.Cedula == model.Contrasenia);
               
                if(resp != null)
                {
                    resp!.Intento = 0;
                    _context.Entry(resp).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return new RsTrxService(StatusCode.Success, 0, "Usuario Desbloqueado.");
                }
                else
                {
                    return responseUnauthorized();
                }
            }
            catch (Exception ex)
            {
                return new RsTrxService(StatusCode.InternalServerError, 0, ex.Message);
            }
        }

        public async Task<IEnumerable<Rq.Usuario>> consultPersonForId(int id)
        {
            var response = await _context.Usuarios.Where(c => id == 0 || c.Id == id).Include(u => u.IdPersonaNavigation).ToListAsync();
            return _mapper.Map<IEnumerable<Rq.Usuario>>(response);
        }

        public async Task<RsTrxService> registerUser(Rq.Usuario model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var modelP = _mapper.Map<Persona>(model.persona);
                _context.Personas.Add(modelP);
                await _context.SaveChangesAsync();

                var modelU = _mapper.Map<Usuario>(model);
                modelU.IdPersona = modelP.Id;
                modelU.Intento = 0;

                //Ref. Definir el rol cuando la ced. exista en esta tabla.
                /*var isDocent = await _context.Docentes.FirstOrDefaultAsync(u => u.Cedula == modelP.Cedula);
                modelU.Rol = (isDocent != null) ? "DOCENTE" : "ESTUDIANTE";*/

                 _context.Usuarios.Add(modelU);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new RsTrxService(StatusCode.Success, 0, "OK");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new RsTrxService(StatusCode.InternalServerError, 0, ex.Message);
            }
        }

        public async Task<RsTrxService> updateUser(Rq.Usuario model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var modelP = _mapper.Map<Persona>(model.persona);
                _context.Personas.Update(modelP);
                await _context.SaveChangesAsync();

                var modelU = _mapper.Map<Usuario>(model);
                modelU.Intento = 0;
                _context.Usuarios.Update(modelU);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new RsTrxService(StatusCode.Success, 0, "Usuario actualizado correctamente.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new RsTrxService(StatusCode.InternalServerError, 0, ex.Message);
            }
        }


        private RsTrxService responseUnauthorized(string message = "Usuario no encontrado o bloqueado.")
        {
            return new RsTrxService(StatusCode.Unauthorized, 0, message);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NAMESPACE
{
    /// <summary>
    /// Repositório padrão básico, provem as funções de comando para os repositórios filhos.
    /// </summary>
    /// <typeparam name="TEntity">Entidade de entrada</typeparam>
    public class Repository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Alias para o contexto padrão ou DefaultContext
        /// </summary>
        protected readonly CoreContext Connection;

        /// <summary>
        /// Construto da classe, o ambiente do banco pode ser definido por ele, alterando o enumerador “Environment”
        /// </summary>
        public Repository()
        {
            Connection = new CoreContext();
        }

        #region Sync

        /// <summary>
        /// Inclui no banco a entidade pre definida no repositório filho, chave primarias com autoincremento não devem ser declaradas para inclusão.
        /// </summary>
        /// <param name="entity">Entidade de entrada</param>
        /// <returns>Retorna um valor positivo para sucesso e negativo para falha. (NOTA: O erro é somente visível no console durante o Debug)</returns>
        public virtual TEntity Insert(TEntity entity)
        {
            try
            {
                TEntity obj = Connection.Set<TEntity>().Add(entity);
                Connection.SaveChanges();
                return obj;
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.ToString());
#endif
                return null;
            }
        }

        /// <summary>
        /// Atualiza no banco a entidade pre definida no repositório filho, a chave primaria deve ser declarada para atualização correta dos dados.
        /// </summary>
        /// <param name="entity">Entidade de entrada</param>
        /// <returns>Retorna um valor positivo para sucesso e negativo para falha. (NOTA: O erro é somente visível no console durante o Debug)</returns>
        public virtual bool Update(TEntity entity)
        {
            try
            {
                Connection.Entry(entity).State = EntityState.Modified;
                Connection.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.ToString());
#endif
                return false;
            }
        }

        /// <summary>
        /// Remove do banco a entidade pre definida no repositório filho, a chave primaria deve ser declarada para remoção do registro correto.
        /// </summary>
        /// <param name="entity">Entidade de entrada</param>
        /// <returns>Retorna um valor positivo para sucesso e negativo para falha. (NOTA: O erro é somente visível no console durante o Debug)</returns>
        public virtual bool Delete(TEntity entity)
        {
            try
            {
                Connection.Entry(entity).State = EntityState.Deleted;
                Connection.Set<TEntity>().Remove(entity);
                Connection.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.ToString());
#endif
                return false;
            }
        }

        /// <summary>
        /// Realiza uma consulta no banco de forma dirigida através de uma expressão Lambda.
        /// </summary>
        /// <param name="expression">Expressão Lambda</param>
        /// <returns>Retorna um “IEnumerable” da entidade referida.</returns>
        public virtual IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> expression)
        {
            return Connection.Set<TEntity>().Where(expression ?? (x => true)).ToList();
        }

        /// <summary>
        /// Realiza uma consulta no banco de forma dirigida através de uma expressão Lambda, porém, suporta buscas cumulativas sendo preferida para uso em filtro de dados
        /// </summary>
        /// <param name="expression">Expressão Lambda</param>
        /// <returns>Retorna um “IQueryable” da entidade referida.</returns>
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> expression)
        {
            return Connection.Set<TEntity>().Where(expression ?? (x => true));
        }

        /// <summary>
        /// Retorna todos os dados da tabela na qual se refere a entidade relacionada (NOTA: Por questão de performasse somente os 1000 primeiros serão coletados)
        /// </summary>
        /// <returns>Retorna um “IEnumerable” da entidade referida.</returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            return Connection.Set<TEntity>().Take(1000).ToList();
        }

        /// <summary>
        /// Retorna o primeiro registro da tabela na qual se refere a entidade relacionada
        /// </summary>
        /// <returns>Retorna a entidade relacionada</returns>
        public virtual TEntity GetOne()
        {
            return Connection.Set<TEntity>().FirstOrDefault();
        }

        /// <summary>
        /// Realiza um busca na tabela na qual se refere a entidade relacionada através da chave primaria
        /// </summary>
        /// <param name="id">Chave primaria</param>
        /// <returns>Retorna a entidade relacionada</returns>
        public virtual TEntity GetById(params object[] id)
        {
            return Connection.Set<TEntity>().Find(id);
        }

        /// <summary>
        /// Realiza uma consulta no banco de forma dirigida através de uma expressão Lambda e retorna somente o primeiro resultado.
        /// </summary>
        /// <param name="expression">Expressão Lambda</param>
        /// <returns>Retorna a entidade relacionada</returns>
        public virtual TEntity GetFirst(Expression<Func<TEntity, bool>> expression)
        {

            return Connection.Set<TEntity>().FirstOrDefault(expression ?? (x => true));
        }

        /// <summary>
        /// Realiza uma consulta no banco de forma dirigida através de uma expressão Lambda e retorna somente o limite solicitado
        /// </summary>
        /// <param name="expression">Expressão Lambda</param>
        /// <param name="limit">Limite para a busca </param>
        /// <returns>Retorna um IEnumerable</returns>
        public virtual IEnumerable<TEntity> GetSome(Expression<Func<TEntity, bool>> expression, int limit)
        {
            return Connection.Set<TEntity>().Where(expression ?? (x => true)).Take(limit).ToList();
        }

        /// <summary>
        /// Conta as linhas da consulta no banco conforme uma expressão Lambda
        /// </summary>
        /// <param name="expression">Expressão Lambda</param>
        /// <returns>Total das contas</returns>
        public virtual int Count(Expression<Func<TEntity, bool>> expression)
        {
            return Connection.Set<TEntity>().Count(expression ?? (x => true));
        }

        #endregion

        #region ASync

        /// <summary>
        /// Inclui no banco a entidade pre definida no repositório filho, chave primarias com autoincremento não devem ser declaradas para inclusão. (Nota: esse método é assíncrono, utilize o comando “await”)
        /// </summary>
        /// <param name="entity">Entidade de entrada</param>
        /// <returns>Retorna um valor positivo para sucesso e negativo para falha. (NOTA: O erro é somente visível no console durante o Debug)</returns>
        public virtual async Task<bool> InsertAsync(TEntity entity)
        {
            try
            {
                Connection.Set<TEntity>().Add(entity);
                await Connection.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.ToString());
#endif
                return false;
            }
        }

        /// <summary>
        /// Atualiza no banco a entidade pre definida no repositório filho, a chave primaria deve ser declarada para atualização correta dos dados. (Nota: esse método é assíncrono, utilize o comando “await”)
        /// </summary>
        /// <param name="entity">Entidade de entrada</param>
        /// <returns>Retorna um valor positivo para sucesso e negativo para falha. (NOTA: O erro é somente visível no console durante o Debug)</returns>
        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            try
            {
                Connection.Entry(entity).State = EntityState.Modified;
                await Connection.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.ToString());
#endif
                return false;
            }

        }

        /// <summary>
        /// Remove do banco a entidade pre definida no repositório filho, a chave primaria deve ser declarada para remoção do registro correto. (Nota: esse método é assíncrono, utilize o comando “await”)
        /// </summary>
        /// <param name="entity">Entidade de entrada</param>
        /// <returns>Retorna um valor positivo para sucesso e negativo para falha. (NOTA: O erro é somente visível no console durante o Debug)</returns>
        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            try
            {
                Connection.Entry(entity).State = EntityState.Deleted;
                Connection.Set<TEntity>().Remove(entity);
                await Connection.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.ToString());
#endif
                return false;
            }
        }
        #endregion
    }
}
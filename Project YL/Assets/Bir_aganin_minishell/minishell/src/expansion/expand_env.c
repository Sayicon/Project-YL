/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   expand_env.c                                       :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: marcudos <marcudos@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2025/05/19 11:22:33 by marcudos          #+#    #+#             */
/*   Updated: 2025/06/01 16:21:42 by marcudos         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/minishell.h"

t_args	*expand_env(char **args, t_env *env, t_shell *sh)
{
	t_args	*head;
	t_args	*cur;
	t_args	*new;
	int		i;

	if (!args || !args[0])
		return (NULL);
	head = expand_token(args[0], env, sh);
	i = 1;
	while (!head && args[i])
		head = expand_token(args[i++], env, sh);
	cur = head;
	while (args[i])
	{
		new = expand_token(args[i], env, sh);
		while (cur->next)
			cur = cur->next;
		cur->next = new;
		i++;
	}
	return (head);
}

char	*start_prefix(char *input, int *i, t_quote *state)
{
	char	*prefix;
	char	*temp;
	char	*new;

	prefix = NULL;
	new = NULL;
	while (input[*i])
	{
		update_state_quote(state, input[*i]);
		if (input[*i] == '\0' || (input[*i] == '$' && *state != SINGLE_QUOTE))
			break ;
		temp = ft_substr(input, *i, 1);
		if (prefix)
		{
			new = ft_strjoin(prefix, temp);
			free(prefix);
			free(temp);
			prefix = new;
		}
		else
			prefix = temp;
		(*i)++;
	}
	return (prefix);
}

void	expand_variable(t_exp *exp, char *key)
{
	char	*temp;

	exp->cur = find_env_value(key, exp->env);
	if (!exp->cur)
		return ;
	if (ft_strcmp(exp->cur, "") == 0)
	{
		free(exp->cur);
		return ;
	}
	if (!exp->prefix)
		exp->prefix = ft_strdup("");
	temp = exp->prefix;
	exp->prefix = ft_strjoin(temp, exp->cur);
	free(temp);
	free(exp->cur);
	exp->cur = NULL;
	if (exp->state == NO_QUOTE)
		handle_no_quotes(exp);
}

void	expand_status(t_exp *exp, int status)
{
	char	*temp;

	exp->cur = ft_itoa(status);
	if (!exp->cur)
		return ;
	if (!exp->prefix)
		exp->prefix = ft_strdup("");
	temp = exp->prefix;
	exp->prefix = ft_strjoin(temp, exp->cur);
	free(temp);
	free(exp->cur);
	exp->cur = NULL;
}

void	handle_no_quotes(t_exp *exp)
{
	char	**parts;
	int		i;

	parts = ft_split(exp->prefix, ' ');
	if (exp->prefix)
		free(exp->prefix);
	exp->prefix = NULL;
	if (count_array((void **) parts) == 1)
		exp->prefix = ft_strdup(parts[0]);
	else
	{
		i = 0;
		while (i == 0 || (parts[i] && parts[i + 1]))
			add_token(exp->head, parts[i++]);
		if (!parts[i])
			exp->prefix = NULL;
		else
			exp->prefix = ft_strdup(parts[i]);
	}
	free_args_temp(parts);
}
